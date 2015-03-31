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
        private const int ModeSelectButton = 7;

        public LiveControllerMode(ControllerUi controllerUi, IClock clock, ILpd8 controller)
            : base(controllerUi, clock, controller) { }

        public override void ProcessButtonEventHandler(ButtonEventArgs e)
        {
            switch(e.ButtonId)
            {
                case ModeSelectButton:
                    if(e.ButtonEventType == ButtonEventType.Released)
                    {
                        HandleSceneSelect();
                    }
                    break;
            }
        }

        private void HandleSceneSelect()
        {
            ControllerUi.GoToSceneSelectMode();
        }

        public override void ProcessControllerChangeEvent(ControllerChangeEventArgs e)
        {
            Controller.SetKeyLightState(e.ControllerId, e.Value >= 128);
        }
    }
}
