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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    public sealed class KeyboardControlBoard<TKeyId> : IControlBoard
    {
        private const double MinControllerValue = 0.0;
        private const double MaxControllerValue = 1.0;

        private readonly VelocityKeyboardEmulator<int, TKeyId> _buttonsKeyboard;
        private readonly ContinuousKeyboardControllerBoardEmulator<int, TKeyId, TimedKey> _controllerBoard;

        public KeyboardControlBoard(
            IKeyboard<TKeyId, TimedKey> baseKeyboard,
            IEnumerable<TKeyId> buttonKeys,
            int defaultVelocity, int highVelocity,
            TKeyId highVelocityKey,
            IEnumerable<TKeyId> controllerKeys,
            IEnumerable<TKeyId> controllerValueKeys,
            TwoSpeedBidirectionalMovementKeysConfiguration<TKeyId> controllerValueChangeKeysConfig,
            double controllerSmallValueStep, double controllerBigValueStep)
        {
            _buttonsKeyboard = new VelocityKeyboardEmulator<int, TKeyId>(
                baseKeyboard,
                CreateKeyMapping(buttonKeys),
                defaultVelocity, highVelocity,
                highVelocityKey
                );

            _controllerBoard = new ContinuousKeyboardControllerBoardEmulator<int, TKeyId, TimedKey>(
                baseKeyboard,
                CreateKeyMapping(controllerKeys),
                controllerValueKeys,
                controllerValueChangeKeysConfig,
                controllerSmallValueStep, controllerBigValueStep
                );
        }

        private static IDictionary<TKeyId, int> CreateKeyMapping(IEnumerable<TKeyId> keys)
        {
            var mapping = new Dictionary<TKeyId, int>();

            var index = 0;
            foreach (var key in keys)
            {
                mapping[key] = index;
                index++;
            }

            return mapping;
        }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return _buttonsKeyboard; } }

        public IControllerBoard<int, double> Controllers { get { return _controllerBoard; } }

        public void SetKeyLightState(int id, bool newState)
        {
            // We don't have lights on the keyboard :(
        }
    }
}
