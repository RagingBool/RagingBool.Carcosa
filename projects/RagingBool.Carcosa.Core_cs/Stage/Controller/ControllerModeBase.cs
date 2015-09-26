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
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal abstract class ControllerModeBase : IControllerMode
    {
        private readonly ControllerUi _controllerUi;
        private readonly IControlBoard _controlBoard;

        private double _lastUpdateTime;

        public ControllerModeBase(ControllerUi controllerUi, IControlBoard controlBoard)
        {
            _controllerUi = controllerUi;
            _controlBoard = controlBoard;
            Fps = 1;
        }

        protected IControlBoard ControlBoard
        {
            get { return _controlBoard; }
        }

        protected ControllerUi ControllerUi
        {
            get { return _controllerUi; }
        }

        protected double Fps { get; set; }

        public virtual void Enter(double time)
        {
            ClearLights();
            _lastUpdateTime = time;
        }

        public virtual void Exit(double time)
        {
            ClearLights();
        }

        public virtual void Update(double time)
        {
            var timeSinceLastUpdate = time - _lastUpdateTime;

            if(timeSinceLastUpdate < (1 / Fps))
            {
                return;
            }

            NewFrame(time);

            _lastUpdateTime = time;
        }

        protected virtual void NewFrame(double time)
        {

        }

        protected void ClearLights()
        {
            for (int i = 0; i < 8; i++)
            {
                _controlBoard.ButtonLights.SetIndicatorValue(i, false);
            }
        }

        public abstract void ProcessButtonEventHandler(KeyEventArgs<int, TimedKeyVelocity> e);
        public abstract void ProcessControllerChangeEvent(ControllerChangeEventArgs<int, double> e);
    }
}
