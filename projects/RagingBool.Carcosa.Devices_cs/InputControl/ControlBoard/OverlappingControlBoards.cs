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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    public sealed class OverlappingControlBoards : IControlBoard
    {
        private readonly IList<IControlBoard> _controlBoards;

        private readonly OverlappingKeyboards<int, TimedKeyVelocity> _buttons;
        private readonly OverlappingControllerBoard<int, double> _controllers;
        private readonly OverlappingIndicatorBoard<int, bool> _buttonLights;

        public OverlappingControlBoards()
        {
            _controlBoards = new List<IControlBoard>();

            _buttons = new OverlappingKeyboards<int, TimedKeyVelocity>();
            _controllers = new OverlappingControllerBoard<int, double>(0);
            _buttonLights = new OverlappingIndicatorBoard<int, bool>(false);
        }

        public void Register(IControlBoard controlBoard)
        {
            _buttons.Register(controlBoard.Buttons);
            _controllers.Register(controlBoard.Controllers);
            _buttonLights.Register(controlBoard.ButtonLights);

            _controlBoards.Add(controlBoard);
        }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return _buttons; } }

        public IControllerBoard<int, double> Controllers { get { return _controllers; } }

        public IIndicatorBoard<int, bool> ButtonLights { get { return _buttonLights; } }
    }
}
