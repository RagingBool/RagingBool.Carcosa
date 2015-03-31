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
    internal sealed class SceneSelectControllerMode : ControllerModeBase
    {
        private static readonly int[] LightSequence = new int[] { 4, 5, 6, 7, 3, 2, 1, 0 };
        
        private int _phase;

        public SceneSelectControllerMode(ControllerUi controllerUi, IClock clock, ILpd8 controller) :
            base(controllerUi, clock, controller) { }

        public override void Enter()
        {
            base.Enter();

            _phase = 0;
            Fps = 20;
        }

        protected override void NewFrame()
        {
            base.NewFrame();

            _phase = (_phase + 1) % 8;
            Render();
        }

        private void Render()
        {
            var currentSceneId = ControllerUi.CurrentSceneId;

            Controller.SetKeyLightState(currentSceneId, true);

            for (int i = 0; i < LightSequence.Length; i++)
            {
                var buttonId = LightSequence[i];

                if (buttonId != currentSceneId)
                {
                    Controller.SetKeyLightState(buttonId, i == _phase);
                }
            }
        }

        public override void ProcessButtonEventHandler(ButtonEventArgs e)
        {
            if(e.ButtonEventType == ButtonEventType.Released)
            {
                ControllerUi.SelectScene(e.ButtonId);
            }
        }

        public override void ProcessControllerChangeEvent(ControllerChangeEventArgs e)
        {
            // Nothing to do here...
        }    
    }
}
