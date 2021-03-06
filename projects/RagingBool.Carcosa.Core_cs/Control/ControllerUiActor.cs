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
using RagingBool.Carcosa.Commons;
using RagingBool.Carcosa.Commons.Control;
using RagingBool.Carcosa.Commons.Control.Akka;
using RagingBool.Carcosa.Core.Stage.Controller;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    public sealed class ControllerUiActor : ControlActor<Unit>
    {
        private readonly ManualControlBoard _controlBoard;
        private ControllerUi _controllerUi;

        public ControllerUiActor()
        {
            _controlBoard = new ManualControlBoard();
            _controllerUi = null;

            _controlBoard.ManualButtonLights.OnIndicatorValueChange += OnButtonLightsValueChange;

            RegisterInputHandler("buttons", HandleButtonsInput);
            RegisterInputHandler("controllers", HandleControllersInput);
        }

        protected override IEnumerable<ControlPortConfiguration> CreateInputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                new ControlPortConfiguration("buttons", typeof(KeyEventArgs<int, TimedKeyVelocity>)),
                new ControlPortConfiguration("controllers", typeof(ControllerChangeEventArgs<int, double>)),
            };
        }

        protected override IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                // TODO
            };
        }

        protected override void OnReceive(object message)
        {
            if (message is StartMessage)
            {
                OnStart((StartMessage)message);
            }
            else if (message is StopMessage)
            {
                OnStop((StopMessage)message);
            }
            else if (message is UpdateMessage)
            {
                OnUpdate((UpdateMessage)message);
            }
            else
            {
                base.OnReceive(message);
            }
        }

        private void OnStart(StartMessage message)
        {
            _controllerUi.Start(message.Time);
        }

        private void OnStop(StopMessage message)
        {
            _controllerUi.Stop(message.Time);
        }

        private void OnUpdate(UpdateMessage message)
        {
            _controllerUi.Update(message.Time);
        }

        protected override void Configure(Unit config)
        {
            if(_controllerUi == null)
            {
                _controllerUi = new ControllerUi(_controlBoard);
            }
        }

        private void HandleButtonsInput(string input, object data)
        {
            var buttonEventArgs = (KeyEventArgs<int, TimedKeyVelocity>)data;
            _controlBoard.ManualButtons.ProcessKeyEvent(buttonEventArgs);
        }

        private void HandleControllersInput(string input, object data)
        {
            var controllerEventArgs = (ControllerChangeEventArgs<int, double>)data;
            _controlBoard.ManualControllers.ProcessControllerChangeEvent(controllerEventArgs);
        }

        private void OnButtonLightsValueChange(object sender, ControllerChangeEventArgs<int, bool> e)
        {
            // TODO
        }
    }
}
