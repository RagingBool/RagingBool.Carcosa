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
using System;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    public sealed class DummyControlBoard : IControlBoard
    {
        private readonly IKeyboard<int, TimedKeyVelocity> _buttons;
        private readonly IControllerBoard<int, double> _controllers;

        public DummyControlBoard()
        {
            _buttons = new DummyKeyboard<int, TimedKeyVelocity>();
            _controllers = new DummyControllerBoard<int, double>(0);
        }

        public int NumberOfButtons { get { return 8; } }
        public int NumberOfControllers { get { return 8; } }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return _buttons; } }
        public IControllerBoard<int, double> Controllers { get { return _controllers; } }

        public void SetKeyLightState(int id, bool newState) { }
    }
}
