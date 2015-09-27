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

using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    public sealed class KeyboardControlBoardButtonsConfig<TKeyId>
    {
        private readonly IEnumerable<TKeyId> _buttonKeys;
        private readonly int _defaultVelocity;
        private readonly int _highVelocity;
        private readonly TKeyId _highVelocityKey;

        public KeyboardControlBoardButtonsConfig(
            IEnumerable<TKeyId> buttonKeys,
            int defaultVelocity, int highVelocity,
            TKeyId highVelocityKey)
        {
            _buttonKeys = buttonKeys;
            _defaultVelocity = defaultVelocity;
            _highVelocity = highVelocity;
            _highVelocityKey = highVelocityKey;
        }

        public IEnumerable<TKeyId> ButtonKeys { get { return _buttonKeys; } }
        public int DefaultVelocity { get { return _defaultVelocity; } }
        public int HighVelocity { get { return _highVelocity; } }
        public TKeyId HighVelocityKey { get { return _highVelocityKey; } }
    }
}
