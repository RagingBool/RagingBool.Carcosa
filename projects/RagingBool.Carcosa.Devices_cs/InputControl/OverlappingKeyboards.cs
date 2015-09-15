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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public sealed class OverlappingKeyboards<TKeyId, TAdditionalKeyEventData> : KeyboardBase<TKeyId, TAdditionalKeyEventData>
    {
        private object _lock = new object();

        private readonly IList<IKeyboard<TKeyId, TAdditionalKeyEventData>> _keyboards;

        public OverlappingKeyboards()
        {
            _keyboards = new List<IKeyboard<TKeyId, TAdditionalKeyEventData>>();
        }

        public void Register(IKeyboard<TKeyId, TAdditionalKeyEventData> keyboard)
        {
            lock(_lock)
            {
                _keyboards.Add(keyboard);

                keyboard.OnKeyEvent += OnKeyEventHandler;
            }
        }

        public void OnKeyEventHandler(object sender, KeyEventArgs<TKeyId, TAdditionalKeyEventData> e)
        {
            lock (_lock)
            {
                ProcessKeyEvent(e);
            }
        }
    }
}
