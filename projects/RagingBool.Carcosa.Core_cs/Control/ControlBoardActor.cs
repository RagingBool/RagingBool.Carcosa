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
using RagingBool.Carcosa.Commons;
using RagingBool.Carcosa.Commons.Control;
using RagingBool.Carcosa.Commons.Control.Akka;
using RagingBool.Carcosa.Devices.InputControl;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    public sealed class ControlBoardActor : ControlActor<Unit>
    {
        private readonly ManualKeyboard<int, TimedKeyVelocity> _inputKeyboard;

        public ControlBoardActor()
        {
            _inputKeyboard = new ManualKeyboard<int, TimedKeyVelocity>();

            RegisterInputHandler("keys", HandleKeysInput);
        }

        protected override IEnumerable<ControlPortConfiguration> CreateInputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                new ControlPortConfiguration("keys", typeof(KeyEventArgs<int, TimedKeyVelocity>))
            };
        }

        protected override IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                // TODO
            };
        }

        protected override void Configure(Unit config) { }

        private void HandleKeysInput(string input, object data)
        {
            var keyEventArgs = (KeyEventArgs<int, TimedKeyVelocity>)data;

            _inputKeyboard.ProcessKeyEvent(keyEventArgs);
        }
    }
}
