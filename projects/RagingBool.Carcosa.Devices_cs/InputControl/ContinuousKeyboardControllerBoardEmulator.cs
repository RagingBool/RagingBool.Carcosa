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

using Epicycle.Commons;
using Epicycle.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public sealed class ContinuousKeyboardControllerBoardEmulator<TControllerId, TKeyId, TAdditionalKeyEventData>
        : KeyboardControllerBoardEmulator<TControllerId, double, TKeyId, TAdditionalKeyEventData>
    {
        public const double MinValue = 0.0;
        public const double MaxValue = 1.0;
        public const double DefaultValue = MinValue;

        private readonly double _smallValueStep;
        private readonly double _bigValueStep;

        public ContinuousKeyboardControllerBoardEmulator(
            IKeyboard<TKeyId, TAdditionalKeyEventData> baseKeyboard,
            IDictionary<TKeyId, TControllerId> controllerKeyMapping,
            IEnumerable<TKeyId> valueKeys,
            TwoSpeedBidirectionalMovementKeysConfiguration<TKeyId> valueChangeKeysConfig,
            double smallValueStep, double bigValueStep)

            : base(
                baseKeyboard,
                DefaultValue,
                controllerKeyMapping,
                GenerateValueKeysMapping(valueKeys),
                valueChangeKeysConfig)
        {
            _smallValueStep = smallValueStep;
            _bigValueStep = bigValueStep;
        }

        private static IDictionary<TKeyId, double> GenerateValueKeysMapping(IEnumerable<TKeyId> valueKeys)
        {
            var valueKeysArray = valueKeys.ToArray();
            var numOfKeys = valueKeysArray.Length;

            if (numOfKeys < 2)
            {
                throw new ArgumentException("There should be at least two value keys");
            }

            var mapping = new Dictionary<TKeyId, double>(numOfKeys);

            // Make sure that the first and the last elements are exactly the Min/Max values
            mapping[valueKeysArray[0]] = MinValue;
            mapping[valueKeysArray[numOfKeys - 1]] = MaxValue;

            // Fill in all the middle values
            var step = (MaxValue - MinValue) / (numOfKeys - 1);
            for (var i = 1; i < (numOfKeys - 1); i++)
            {
                mapping[valueKeysArray[i]] = MinValue + step * i;
            }

            return mapping;
        }

        protected override double AdvanceValue(double value, bool isForwardOrBackward, bool isBigOrSmall)
        {
            var step = isBigOrSmall ? _bigValueStep : _smallValueStep;
            if(!isForwardOrBackward)
            {
                step = -step;
            }

            return BasicMath.Clip(value + step, MinValue, MaxValue);
        }
    }
}
