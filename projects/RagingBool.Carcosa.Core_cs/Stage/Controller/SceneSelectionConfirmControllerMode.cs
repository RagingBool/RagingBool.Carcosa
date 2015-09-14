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
    internal sealed class SceneSelectionConfirmControllerMode : ControllerModeBase
    {
        private const double Duration = 0.15;

        private bool _isOn;
        private double _startTime;

        public SceneSelectionConfirmControllerMode(ControllerUi controllerUi, IClock clock, ILpd8 controller) :
            base(controllerUi, clock, controller) { }

        public override void Enter()
        {
            base.Enter();

            _isOn = true;
            Fps = 40;

            _startTime = Clock.Time;
        }

        protected override void NewFrame()
        {
            base.NewFrame();

            var elapsedTime = Clock.Time - _startTime;
            if(elapsedTime > Duration)
            {
                ControllerUi.GoToLiveMode();
                return;
            }

            _isOn = !_isOn;
            Render();
        }

        private void Render()
        {
            var currentSceneId = ControllerUi.SceneId;

            Controller.SetKeyLightState(currentSceneId, _isOn);
        }

        public override void ProcessButtonEventHandler(KeyEventArgs<int, KeyVelocity> e)
        {
            // Nothing to do here...
        }

        public override void ProcessControllerChangeEvent(ControllerChangeEventArgs e)
        {
            // Nothing to do here...
        }
    }
}
