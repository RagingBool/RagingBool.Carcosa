﻿// [[[[INFO>
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

using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal sealed class ControllerUi : IControllerUi
    {
        private readonly object _lock = new object();

        private readonly IControlBoard _controlBoard;

        private LiveControllerMode _liveMode;
        private SceneSelectControllerMode _sceneSelectMode;
        private SceneSelectionConfirmControllerMode _SceneSelectionConfirmMode;

        private IControllerMode _mode;

        private SceneChangedEventArgs _lastSceneChangedEventArgs;

        public ControllerUi(IControlBoard controlBoard)
        {
            _controlBoard = controlBoard;

            _controlBoard.Buttons.OnKeyEvent += OnButtonEventHandler;
            _controlBoard.Controllers.OnControllerChangeEvent += OnControllerChangeHandler;

            _liveMode = new LiveControllerMode(this, _controlBoard);
            _sceneSelectMode = new SceneSelectControllerMode(this, _controlBoard);
            _SceneSelectionConfirmMode = new SceneSelectionConfirmControllerMode(this, _controlBoard);

            _mode = null;
            _lastSceneChangedEventArgs = null;
        }

        public void Start(double time)
        {
            lock(_lock)
            {
                SceneId = -1;
                SelectSceneAndGoToLiveMode(0, time);
            }
        }

        public void Stop(double time)
        {
            lock (_lock)
            {
                SwitchMode(null, time);
            }
        }

        public void Update(double time)
        {
            lock (_lock)
            {
                if (_mode != null)
                {
                    _mode.Update(time);
                }
            }
        }

        private void SwitchMode(IControllerMode newMode, double time)
        {
            lock (_lock)
            {
                if (_mode != null)
                {
                    _mode.Exit(time);
                }

                _mode = newMode;

                if (_mode != null)
                {
                    _mode.Enter(time);
                }
            }
        }

        private void OnButtonEventHandler(object sender, KeyEventArgs<int, TimedKeyVelocity> e)
        {
            lock (_lock)
            {
                if (_mode != null)
                {
                    _mode.ProcessButtonEventHandler(e);
                }
            }
        }

        private void OnControllerChangeHandler(object sender, ControllerChangeEventArgs<int, double> e)
        {
            lock (_lock)
            {
                if (_mode != null)
                {
                    _mode.ProcessControllerChangeEvent(e);
                }
            }
        }

        public int SceneId { get; private set; }
        public int SubsceneId
        {
            get { return _liveMode.SubsceneId; }
        }

        public void SelectSceneAndGoToLiveMode(int newSceneId, double time)
        {
            lock (_lock)
            {
                if (SceneId != newSceneId)
                {
                    SceneId = newSceneId;
                    _liveMode.SubsceneId = 0;

                    SwitchMode(_SceneSelectionConfirmMode, time);
                    FireOnSceneChange();
                }
                else
                {
                    GoToLiveMode(time);
                }
            }
        }

        public void GoToLiveMode(double time)
        {
            SwitchMode(_liveMode, time);
        }

        public void GoToSceneSelectMode(double time)
        {
            SwitchMode(_sceneSelectMode, time);
        }

        public void FireOnSceneChange()
        {
            if(OnSceneChange != null)
            {
                var eventArgs = new SceneChangedEventArgs(SceneId, SubsceneId);

                if (_lastSceneChangedEventArgs != null && 
                    eventArgs.NewSceneId == _lastSceneChangedEventArgs.NewSceneId &&
                    eventArgs.NewSubsceneId == _lastSceneChangedEventArgs.NewSubsceneId)
                {
                    return;
                }

                _lastSceneChangedEventArgs = eventArgs;

                OnSceneChange(this, eventArgs);
            }
        }

        public event EventHandler<SceneChangedEventArgs> OnSceneChange;

        public void FireLightDrumEvent(LightDrumEventArgs eventArgs)
        {
            if(OnLightDrumEvent != null)
            {
                OnLightDrumEvent(this, eventArgs);
            }
        }

        public event EventHandler<LightDrumEventArgs> OnLightDrumEvent;

        public void FireControlParameterValueChange(ControlParameterValueChangeEventArgs eventArgs)
        {
            if (OnControlParameterValueChange != null)
            {
                OnControlParameterValueChange(this, eventArgs);
            }
        }

        public event EventHandler<ControlParameterValueChangeEventArgs> OnControlParameterValueChange;
    }
}
