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
    public sealed class KeyboardControlBoardControllersConfig<TKeyId>
    {
        private readonly IEnumerable<TKeyId> _controllerKeys;
        private readonly IEnumerable<TKeyId> _valueKeys;
        private readonly TwoSpeedBidirectionalMovementKeysConfiguration<TKeyId> _valueChangeKeysConfig;
        private readonly double _smallValueStep;
        private readonly double _bigValueStep;

        public KeyboardControlBoardControllersConfig(
            IEnumerable<TKeyId> controllerKeys,
            IEnumerable<TKeyId> valueKeys,
            TwoSpeedBidirectionalMovementKeysConfiguration<TKeyId> valueChangeKeysConfig,
            double smallValueStep, double bigValueStep)
        {
            _controllerKeys = controllerKeys;
            _valueKeys = valueKeys;
            _valueChangeKeysConfig = valueChangeKeysConfig;
            _smallValueStep = smallValueStep;
            _bigValueStep = bigValueStep;
        }

        public IEnumerable<TKeyId> ControllerKeys { get { return _controllerKeys; } }
        public IEnumerable<TKeyId> ValueKeys { get { return _valueKeys; } }
        public TwoSpeedBidirectionalMovementKeysConfiguration<TKeyId> ValueChangeKeysConfig { get { return _valueChangeKeysConfig; } }
        public double SmallValueStep { get { return _smallValueStep; } }
        public double BigValueStep { get { return _bigValueStep; } }
    }
}
