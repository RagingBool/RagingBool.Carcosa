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

using RagingBool.Carcosa.Core.Stage.Controller;
using RagingBool.Carcosa.Core.Stage.Lights;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    class FadecandyScene : SceneBase
    {
        private readonly LightSetup _lightSetup;

        private int _monoState;
        private bool _isOff;
        private bool _isFull;

        public FadecandyScene(LightSetup lightSetup)
        {
            _lightSetup = lightSetup;
            _monoState = 0;
        }

        public override void Enter()
        {
            _isOff = false;
            _isFull = false;
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
            UpdateStrip(_lightSetup.FadecandyStripAll, 0, 4, 3, 7);
            //UpdateStrip(_lightSetup.FadecandyStrip1, 0, 4, 3, 7);
            //UpdateStrip(_lightSetup.FadecandyStrip2, 1, 5, 3, 7);
        }

        private void UpdateStrip(IList<IRgbLight> lights, int hue1Knob, int hue2Knob, int satKnob, int intensityKnob)
        {
            var saturation = GetControl(satKnob);
            var intensity = GetControl(intensityKnob);
            
            var midHue = GetControl(hue1Knob);
            var hueOpening = GetControl(hue2Knob);

            LightUtils.SetHueGradient(lights, midHue, hueOpening, saturation, intensity);
        }

        public override void HandleSubsceneChange(int newSubscene)
        {
        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
        }
    }
}
