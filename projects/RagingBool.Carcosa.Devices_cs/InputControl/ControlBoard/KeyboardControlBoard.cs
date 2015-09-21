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
using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    public sealed class KeyboardControlBoard<TKeyId> : IControlBoard
    {
        private const double MinControllerValue = 0.0;
        private const double MaxControllerValue = 1.0;

        private readonly KeyboardControlBoardButtons<TKeyId> _buttonsKeyboard;
        private readonly KeyboardControlBoardControllers<TKeyId> _controllerBoard;
        private readonly DummyIndicatorBoard<int, bool> _buttonLights;

        public KeyboardControlBoard(
            IKeyboard<TKeyId, TimedKey> baseKeyboard,
            KeyboardControlBoardConfig<TKeyId> config)
        {
            _buttonsKeyboard = new KeyboardControlBoardButtons<TKeyId>(baseKeyboard, config.KeyboardConfig);
            _controllerBoard = new KeyboardControlBoardControllers<TKeyId>(baseKeyboard, config.ControllerConfig);
            _buttonLights = new DummyIndicatorBoard<int, bool>(false);
        }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return _buttonsKeyboard; } }

        public IControllerBoard<int, double> Controllers { get { return _controllerBoard; } }

        public IIndicatorBoard<int, bool> ButtonLights { get { return _buttonLights; } }
    }
}
