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
using System;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public abstract class KeyboardBase<TKeyId, TAdditionalKeyEventData> : IKeyboard<TKeyId, TAdditionalKeyEventData>
    {
        private readonly Dictionary<TKeyId, KeyState> _keyStates;

        public KeyboardBase()
        {
            _keyStates = new Dictionary<TKeyId, KeyState>();
        }

        protected void ProcessKeyEvent(KeyEventArgs<TKeyId, TAdditionalKeyEventData> eventArgs)
        {
            var keyId = eventArgs.KeyId;
            var eventType = eventArgs.EventType;

            var prevState = GetKeyState(keyId);
            var newState = eventType != KeyEventType.Released ? KeyState.Pressed : KeyState.Released;

            _keyStates[keyId] = newState;

            if (OnKeyEvent != null && eventType == KeyEventType.Repeat || prevState != newState)
            {
                OnKeyEvent(this, eventArgs);
            }
        }

        public event EventHandler<KeyEventArgs<TKeyId, TAdditionalKeyEventData>> OnKeyEvent;

        public KeyState GetKeyState(TKeyId keyId)
        {
            if (!_keyStates.ContainsKey(keyId))
            {
                return KeyState.Released;
            }

            return _keyStates[keyId];
        }
    }
}
