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
    internal sealed class SceneSelectControllerMode : ControllerModeBase
    {
        private static readonly int[] LightSequence = new int[] { 4, 5, 6, 7, 3, 2, 1, 0 };
        
        private int _phase;

        public SceneSelectControllerMode(ControllerUi controllerUi, IControlBoard controlBoard) :
            base(controllerUi, controlBoard) { }

        public override void Enter(double time)
        {
            base.Enter(time);

            _phase = 0;
            Fps = 20;
        }

        protected override void NewFrame(double time)
        {
            base.NewFrame(time);

            _phase = (_phase + 1) % 8;
            Render();
        }

        private void Render()
        {
            var currentSceneId = ControllerUi.SceneId;

            ControlBoard.ButtonLights.SetIndicatorValue(currentSceneId, true);

            for (int i = 0; i < LightSequence.Length; i++)
            {
                var buttonId = LightSequence[i];

                if (buttonId != currentSceneId)
                {
                    ControlBoard.ButtonLights.SetIndicatorValue(buttonId, i == _phase);
                }
            }
        }

        public override void ProcessButtonEventHandler(KeyEventArgs<int, TimedKeyVelocity> e)
        {
            if(e.EventType == KeyEventType.Released)
            {
                ControllerUi.SelectSceneAndGoToLiveMode(e.KeyId, e.AdditionalData.Time);
            }
        }

        public override void ProcessControllerChangeEvent(ControllerChangeEventArgs<int, double> e)
        {
            // Nothing to do here...
        }    
    }
}
