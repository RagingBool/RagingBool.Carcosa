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

using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Commons.Control;       
using RagingBool.Carcosa.Commons.Control.Akka;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    internal sealed class KeyboardControlBoardButtonsActor<TInputKeyId>
        : ControlActor<KeyboardControlBoardButtonsConfig<TInputKeyId>>
    {
        private readonly ManualKeyboard<TInputKeyId, TimedKey> _inputKeyboard;
        private KeyboardControlBoardButtons<TInputKeyId> _keyboard;

        public KeyboardControlBoardButtonsActor()
        {
            _inputKeyboard = new ManualKeyboard<TInputKeyId, TimedKey>();
            _keyboard = null;

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
                new ControlPortConfiguration("", typeof(KeyEventArgs<int, TimedKeyVelocity>))
            };
        }

        protected override void Configure(KeyboardControlBoardButtonsConfig<TInputKeyId> config)
        {
            if (_keyboard != null)
            {
                _keyboard.OnKeyEvent -= OnKeyEventHandler;
            }

            _keyboard = new KeyboardControlBoardButtons<TInputKeyId>(_inputKeyboard, config);
            _keyboard.OnKeyEvent += OnKeyEventHandler;
        }

        private void HandleInput(string input, object data)
        {
            var keyEventArgs = (KeyEventArgs<TInputKeyId, TimedKey>)data;

            _inputKeyboard.ProcessKeyEvent(keyEventArgs);
        }

        private void OnKeyEventHandler(object sender, KeyEventArgs<int, TimedKeyVelocity> e)
        {
            Output("", e);
        }
    }
}
