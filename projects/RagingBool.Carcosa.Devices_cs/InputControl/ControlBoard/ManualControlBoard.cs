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
    public sealed class ManualControlBoard : IControlBoard
    {
        private ManualKeyboard<int, TimedKeyVelocity> _buttons;
        private ManualControllerBoard<int, double> _controllers;
        private ManualIndicatorBoard<int, bool> _buttonLights;

        public ManualControlBoard()
        {
            _buttons = new ManualKeyboard<int, TimedKeyVelocity>();
            _controllers = new ManualControllerBoard<int, double>(0);
            _buttonLights = new ManualIndicatorBoard<int, bool>(false);
        }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return ManualButtons; } }

        public IControllerBoard<int, double> Controllers { get { return ManualControllers; } }

        public IIndicatorBoard<int, bool> ButtonLights { get { return ManualButtonLights; } }

        public ManualKeyboard<int, TimedKeyVelocity> ManualButtons { get { return _buttons; } }

        public ManualControllerBoard<int, double> ManualControllers { get { return _controllers; } }

        public ManualIndicatorBoard<int, bool> ManualButtonLights { get { return _buttonLights; } }
    }
}
