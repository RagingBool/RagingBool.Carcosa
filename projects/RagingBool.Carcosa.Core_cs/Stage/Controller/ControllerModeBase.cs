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

using Epicycle.Commons.Time;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.Lpd8;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal abstract class ControllerModeBase : IControllerMode
    {
        private readonly ControllerUi _controllerUi;
        private readonly IClock _clock;
        private readonly ILpd8 _controller;

        private double _lastUpdateTime;

        public ControllerModeBase(ControllerUi controllerUi, IClock clock, ILpd8 controller)
        {
            _controllerUi = controllerUi;
            _clock = clock;
            _controller = controller;
            Fps = 1;
        }

        protected ILpd8 Controller
        {
            get { return _controller; }
        }

        protected IClock Clock
        {
            get { return _clock; }
        }

        protected ControllerUi ControllerUi
        {
            get { return _controllerUi; }
        }

        protected double Fps { get; set; }

        public virtual void Enter()
        {
            ClearLights();
            _lastUpdateTime = _clock.Time;
        }

        public virtual void Exit()
        {
            ClearLights();
        }
        
        public virtual void Update()
        {
            var curTime = _clock.Time;
            var timeSinceLastUpdate = curTime - _lastUpdateTime;

            if(timeSinceLastUpdate < (1 / Fps))
            {
                return;
            }

            NewFrame();

            _lastUpdateTime = curTime;
        }

        protected virtual void NewFrame()
        {

        }

        protected void ClearLights()
        {
            for (int i = 0; i < _controller.NumberOfButtons; i++)
            {
                _controller.SetKeyLightState(i, false);
            }
        }

        public abstract void ProcessButtonEventHandler(KeyEventArgs<int, KeyVelocity> e);
        public abstract void ProcessControllerChangeEvent(ControllerChangeEventArgs e);
    }
}
