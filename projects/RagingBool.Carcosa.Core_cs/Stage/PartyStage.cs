// [[[[INFO>
// Copyright 2015 Raging Bool (http://ragingbool.org, https://github.com/RagingBool)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/RagingBool/RagingBool.Carcosa
// ]]]]

using Epicycle.Commons.Time;
using RagingBool.Carcosa.Core.Stage.Controller;
using RagingBool.Carcosa.Core.Stage.Scenes;
using RagingBool.Carcosa.Core.Stage.Scenes.Forest;
using RagingBool.Carcosa.Core.Workspace;
using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using RagingBool.Carcosa.Devices.LightControl;
using RagingBool.Carcosa.Devices.LightControl.Dmx;
using RagingBool.Carcosa.Devices.LightControl.Opc;
using RagingBool.Carcosa.Devices.Midi;
using System;

namespace RagingBool.Carcosa.Core.Stage
{
    internal sealed class PartyStage : IStage
    {
        private const double SceneUpdateTimeThreshold = 0.01;

        private readonly object _lock = new object();

        private readonly IClock _clock;

        private IDevice _controlBoardDevice;
        private IUpdatable _controlBoardUpdatable;
        private IControlBoard _controlBoard;

        private E1_31DmxMultiverse _e1_31DmxMultiverse;
        private IBufferedLightController _dmxUniverseController1;
        private MaxFrequencyUpdater _dmxMultiverseUpdater1;

        private SerialOpcDevice _snark;
        private IBufferedLightController _snarkController;
        private MaxFrequencyUpdater _snarkUpdater;

        private NetworkOpcDevice _fadecandy;
        private IBufferedLightController _fadecandyContoller;
        private MaxFrequencyUpdater _fadecandyUpdater;

        private IScene _curScene;
        private int _curSceneId;

        private LightSetup _lightSetup;

        private PartyScene1 _partyScene1;
        private FadecandyScene _fadecandyScene;
        private ForestScene _forestScene;
        private ManualScene _manualScene;

        private readonly ControllerUi _controllerUi;

        private double _lastSceneUpdate;

        public PartyStage(IClock clock, ICarcosaWorkspace workspace)
        {
            _clock = clock;

            InitController(workspace, false);
            InitDMX(workspace, false);
            InitForestOpcDevice(workspace, false);
            InitSnark(workspace, false);

            _controllerUi = new ControllerUi(_clock, _controlBoard);

            _controllerUi.OnSceneChange += OnSceneChange;
            _controllerUi.OnLightDrumEvent += OnLightDrumEvent;
            _controllerUi.OnControlParameterValueChange += OnControlParameterValueChange;

            _lightSetup = new LightSetup(_dmxUniverseController1, _snarkController, _fadecandyContoller);

            _partyScene1 = new PartyScene1(_clock, _lightSetup);
            _fadecandyScene = new FadecandyScene(_lightSetup);
            _forestScene = new ForestScene(_lightSetup);
            _manualScene = new ManualScene(_lightSetup);

            _curScene = null;
            _curSceneId = -1;
        }

        private void InitController(ICarcosaWorkspace workspace, bool isOn)
        {
            if (isOn)
            {
                var controller = new MidiLpd8(_clock, workspace.ControllerMidiInPort, workspace.ControllerMidiOutPort);

                _controlBoard = controller;
                _controlBoardUpdatable = controller;
                _controlBoardDevice = controller;
            }
            else
            {
                _controlBoard = new DummyControlBoard();
                _controlBoardUpdatable = null;
                _controlBoardDevice = null;
            }
        }

        private void InitDMX(ICarcosaWorkspace workspace, bool isOn)
        {
            if(isOn)
            {
                var componentIdentifier = Guid.NewGuid();
                var sourceName = "test";
                _e1_31DmxMultiverse = new E1_31DmxMultiverse(new int[] { 1 }, componentIdentifier, sourceName);
                _dmxUniverseController1 = new BufferedLightController(new FramedDmxController(_e1_31DmxMultiverse.GetUniverse(1)));
                _dmxMultiverseUpdater1 = new MaxFrequencyUpdater(_dmxUniverseController1, _clock, 30.0);
            }
            else
            {
                _e1_31DmxMultiverse = null;
                _dmxUniverseController1 = null;
                _dmxMultiverseUpdater1 = null;
            }
        }

        private void InitForestOpcDevice(ICarcosaWorkspace workspace, bool isOn)
        {
            if(isOn)
            {
                var host = "forest";
                _fadecandy = new NetworkOpcDevice(host, 7890);
                _fadecandyContoller = new BufferedLightController(new FramedOpcController(_fadecandy, 0, 480 * 3));
                _fadecandyUpdater = new MaxFrequencyUpdater(_fadecandyContoller, _clock, 60.0);
            }
            else
            {
                _fadecandy = null;
                _fadecandyContoller = null;
                _fadecandyUpdater = null;
            }
        }

        private void InitSnark(ICarcosaWorkspace workspace, bool isOn)
        {
            if(isOn)
            {
                _snark = new SerialOpcDevice(workspace.SnarkSerialPortName);
                _snarkController = new BufferedLightController(new FramedOpcController(_snark, 0, 12));
                _snarkUpdater = new MaxFrequencyUpdater(_snarkController, _clock, 50.0);
            }
            else
            {
                _snark = null;
                _snarkController = null;
                _snarkUpdater = null;
            }
        }

        public void Start()
        {
            lock (_lock)
            {
                if (_controlBoardDevice != null)
                {
                    _controlBoardDevice.Connect();
                }

                if (_e1_31DmxMultiverse != null)
                {
                    _e1_31DmxMultiverse.Connect();
                }

                if (_snark != null)
                {
                    _snark.Connect();
                }

                if(_fadecandy != null)
                {
                    _fadecandy.Connect();
                }

                _controllerUi.Start();

                _lastSceneUpdate = _clock.Time;
            }
        }

        public void Update()
        {
            lock (_lock)
            {
                if (_controlBoardUpdatable != null)
                {
                    _controlBoardUpdatable.Update();
                }

                if (_dmxMultiverseUpdater1 != null)
                {
                    _dmxMultiverseUpdater1.Update();
                }

                if (_snarkUpdater != null)
                {
                    _snarkUpdater.Update();
                }

                if(_fadecandy != null)
                {
                    _fadecandyUpdater.Update();
                }

                _controllerUi.Update();
                UpdateScene();
            }
        }

        private void UpdateScene()
        {
            if(_curScene == null)
            {
                return;
            }

            var time = _clock.Time;
            var dt = time - _lastSceneUpdate;

            if(dt >= SceneUpdateTimeThreshold)
            {
                _curScene.Update(dt);
                _lastSceneUpdate = time;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                _controllerUi.Stop();

                if (_controlBoardDevice != null)
                {
                    _controlBoardDevice.Disconnect();
                }

                if(_snark != null)
                {
                    _snark.Disconnect();                    
                }
            }
        }

        private void SetScene(IScene newScene, int newSceneId)
        {
            if(_curScene != null)
            {
                _curScene.Exit();
            }

            _curScene = newScene;
            _curSceneId = newSceneId;

            if (_curScene != null)
            {
                _curScene.Enter();
            }
        }

        private void OnSceneChange(object sender, SceneChangedEventArgs eventArgs)
        {
            var newSceneId = eventArgs.NewSceneId;

            if (newSceneId != _curSceneId)
            {
                switch (newSceneId)
                {
                    case 0:
                        SetScene(_partyScene1, newSceneId);
                        break;
                    case 1:
                        SetScene(_fadecandyScene, newSceneId);
                        break;
                    case 6:
                        SetScene(_forestScene, newSceneId);
                        break;
                    default:
                        SetScene(_manualScene, newSceneId);
                        break;
                }
            }
            else
            {
                if (_curScene != null)
                {
                    _curScene.HandleSubsceneChange(eventArgs.NewSubsceneId);
                }
            }
        }

        private void OnLightDrumEvent(object sender, LightDrumEventArgs eventArgs)
        {
            if (_curScene != null)
            {
                _curScene.HandleLightDrumEvent(eventArgs);
            }
        }

        private void OnControlParameterValueChange(object sender, ControlParameterValueChangeEventArgs eventArgs)
        {
            if (_curScene != null)
            {
                _curScene.HandleControlParameterValueChange(eventArgs);
            }
        }
    }
}
