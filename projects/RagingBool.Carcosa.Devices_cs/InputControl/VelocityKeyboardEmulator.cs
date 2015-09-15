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
    public class VelocityKeyboardEmulator<TKeyId, TBaseKeyId> : KeyboardBase<TKeyId, TimedKeyVelocity>
    {
        private readonly IDictionary<TBaseKeyId, TKeyId> _keyMapping;
        private readonly IKeyboard<TBaseKeyId, TimedKey> _baseKeyboard;
        private readonly int _defaultVelocity;
        private readonly int _highVelocity;
        private readonly TBaseKeyId _highVelocityKey;

        public VelocityKeyboardEmulator(
            IKeyboard<TBaseKeyId, TimedKey> baseKeyboard,
            IDictionary<TBaseKeyId, TKeyId> keyMapping,
            int defaultVelocity, int highVelocity,
            TBaseKeyId highVelocityKey)
        {
            _baseKeyboard = baseKeyboard;

            _keyMapping = keyMapping;
            _defaultVelocity = defaultVelocity;
            _highVelocity = highVelocity;
            _highVelocityKey = highVelocityKey;

            _baseKeyboard.OnKeyEvent += OnKeyEventHandler;
        }

        private void OnKeyEventHandler(object sender, KeyEventArgs<TBaseKeyId, TimedKey> e)
        {
            if (e.EventType == KeyEventType.Repeat)
            {
                return;
            }

            if (!_keyMapping.ContainsKey(e.KeyId))
            {
                return;
            }

            var keyId = _keyMapping[e.KeyId];

            var eventType = e.EventType;

            var isHighVelocity = _baseKeyboard.GetKeyState(_highVelocityKey) == KeyState.Pressed;
            var velocity = isHighVelocity ? _highVelocity : _defaultVelocity;

            var timedKeyVelocity = new TimedKeyVelocity(e.AdditionalData.Time, velocity);
            var outEvent = new KeyEventArgs<TKeyId, TimedKeyVelocity>(keyId, eventType, timedKeyVelocity);

            ProcessKeyEvent(outEvent);
        }
    }
}
