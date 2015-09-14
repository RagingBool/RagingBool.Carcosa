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

using Epicycle.Input;
using Epicycle.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RagingBool.Carcosa.App
{
    public class WpfKeyboardManager : IKeyboard<Key, Unit>
    {
        private readonly Dictionary<Key, KeyState> _keyStates;

        public WpfKeyboardManager()
        {
            _keyStates = new Dictionary<Key, KeyState>();
        }

        public event EventHandler<KeyEventArgs<Key, Unit>> OnKeyEvent;

        public KeyState GetKeyState(Key keyId)
        {
            if(!_keyStates.ContainsKey(keyId))
            {
                return KeyState.Released;
            }

            return _keyStates[keyId];
        }

        public void ProcessWpfKeyboardEvent(System.Windows.Input.KeyEventArgs eventArgs)
        {
            var keyId = eventArgs.Key;
            var eventType = CalcEventType(eventArgs);

            var prevState = GetKeyState(keyId);
            var newState = eventType != KeyEventType.Released ? KeyState.Pressed : KeyState.Released;

            _keyStates[keyId] = newState;

            if (OnKeyEvent != null && eventType == KeyEventType.Repeat || prevState != newState)
            {
                var outgoingEvent = new KeyEventArgs<Key, Unit>(keyId, eventType, Unit.Instance);
                OnKeyEvent(this, outgoingEvent);
            }
        }

        private static KeyEventType CalcEventType(System.Windows.Input.KeyEventArgs eventArgs)
        {
            if(eventArgs.IsRepeat)
            {
                return KeyEventType.Repeat;
            }
            
            if(eventArgs.IsDown)
            {
                return KeyEventType.Pressed;
            }

            if(eventArgs.IsUp)
            {
                return KeyEventType.Released;
            }

            throw new ArgumentException("Illegal key event!");
        }
    }
}
