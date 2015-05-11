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
using RagingBool.Carcosa.Devices.Dmx;
using RagingBool.Carcosa.Devices.Fadecandy;
using RagingBool.Carcosa.Devices.Midi;

namespace RagingBool.Carcosa.Core.Stage
{
    internal sealed class PartyStage : IStage
    {
        private const double SceneUpdateTimeThreshold = 0.01;

        private readonly object _lock = new object();

        private readonly IClock _clock;

        private readonly ILpd8 _controller;
        private readonly IDmxMultiverse _dmxMultiverse;
        private readonly ISnark _snark;
        private readonly FadecandyOpenPixelClient _fadecandyClient;

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

            _controller = new MidiLpd8(workspace.ControllerMidiInPort, workspace.ControllerMidiOutPort);
            
            _dmxMultiverse = new E1_31DmxMultiverse(_clock, 30.0);
            _dmxMultiverse.AddUniverse(1);

            _snark = new SerialSnark(_clock, workspace.SnarkSerialPortName, 12, 50);
            var host = "forest";
            _fadecandyClient = new FadecandyOpenPixelClient(_clock, host, 7890, 480 * 3, 60.0);

            _controllerUi = new ControllerUi(_clock, _controller);

            _controllerUi.OnSceneChange += OnSceneChange;
            _controllerUi.OnLightDrumEvent += OnLightDrumEvent;
            _controllerUi.OnControlParameterValueChange += OnControlParameterValueChange;

            _lightSetup = new LightSetup(_dmxMultiverse, _snark, _fadecandyClient);

            _partyScene1 = new PartyScene1(_clock, _lightSetup);
            _fadecandyScene = new FadecandyScene(_lightSetup);
            _forestScene = new ForestScene(_lightSetup);
            _manualScene = new ManualScene(_lightSetup);

            _curScene = null;
            _curSceneId = -1;
        }

        public void Start()
        {
            lock (_lock)
            {
                _controller.Connect();
                _dmxMultiverse.Connect();
                _snark.Connect();

                _controllerUi.Start();

                _lastSceneUpdate = _clock.Time;
            }
        }

        public void Update()
        {
            lock (_lock)
            {
                _controller.Update();
                _dmxMultiverse.Update();
                _snark.Update();
                _fadecandyClient.Update();

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

                _controller.Disconnect();
                _dmxMultiverse.Disconnect();
                _snark.Disconnect();
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
