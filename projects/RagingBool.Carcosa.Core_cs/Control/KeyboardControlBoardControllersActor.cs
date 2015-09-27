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

using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Commons.Control;
using RagingBool.Carcosa.Commons.Control.Akka;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    internal sealed class KeyboardControlBoardControllersActor<TInputKeyId>
        : ControlActor<KeyboardControlBoardControllersConfig<TInputKeyId>>
    {
        private readonly ManualKeyboard<TInputKeyId, TimedKey> _inputKeyboard;
        private KeyboardControlBoardControllers<TInputKeyId> _controllers;

        public KeyboardControlBoardControllersActor()
        {
            _inputKeyboard = new ManualKeyboard<TInputKeyId, TimedKey>();
            _controllers = null;

            RegisterInputHandler("", HandleInput);
        }

        protected override IEnumerable<ControlPortConfiguration> CreateInputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                new ControlPortConfiguration("", typeof(KeyEventArgs<TInputKeyId, TimedKey>))
            };
        }

        protected override IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                new ControlPortConfiguration("", typeof(ControllerChangeEventArgs<int, double>))
            };
        }

        protected override void Configure(KeyboardControlBoardControllersConfig<TInputKeyId> config)
        {
            if (_controllers != null)
            {
                _controllers.OnControllerChangeEvent -= OnControllerChangeEventHandler;
            }

            _controllers = new KeyboardControlBoardControllers<TInputKeyId>(_inputKeyboard, config);
            _controllers.OnControllerChangeEvent += OnControllerChangeEventHandler;
        }

        private void HandleInput(string input, object data)
        {
            var keyEventArgs = (KeyEventArgs<TInputKeyId, TimedKey>)data;

            _inputKeyboard.ProcessKeyEvent(keyEventArgs);
        }

        private void OnControllerChangeEventHandler(object sender, ControllerChangeEventArgs<int, double> e)
        {
            Output("", e);
        }
    }
}
