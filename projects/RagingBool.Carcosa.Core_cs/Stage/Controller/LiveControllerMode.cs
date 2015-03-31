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
using RagingBool.Carcosa.Devices;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal sealed class LiveControllerMode : ControllerModeBase
    {
        private const int SceneSelectButton = 7;
        private const int SubsceneSelectButton = 3;

        private readonly SubsceneSelector _subsceneSelector;

        public LiveControllerMode(ControllerUi controllerUi, IClock clock, ILpd8 controller)
            : base(controllerUi, clock, controller)
        {
            _subsceneSelector = new SubsceneSelector(Controller, SubsceneSelectButton);
        }

        public override void Enter()
        {
            Fps = 60;
        }

        protected override void NewFrame()
        {
            base.NewFrame();

            _subsceneSelector.NewFrame();
        }

        public override void ProcessButtonEventHandler(ButtonEventArgs e)
        {
            switch(e.ButtonId)
            {
                case SceneSelectButton:
                    if(e.ButtonEventType == ButtonEventType.Released)
                    {
                        HandleSceneSelect();
                    }
                    break;
                case SubsceneSelectButton:
                    if(e.ButtonEventType == ButtonEventType.Pressed)
                    {
                        HandleSubsceneSelect();
                    }
                    break;
            }
        }

        private void HandleSceneSelect()
        {
            ControllerUi.GoToSceneSelectMode();
        }

        private void HandleSubsceneSelect()
        {
            _subsceneSelector.SelectNextSubscene();
        }

        public override void ProcessControllerChangeEvent(ControllerChangeEventArgs e)
        {
            Controller.SetKeyLightState(e.ControllerId, e.Value >= 128);
        }
    }
}
