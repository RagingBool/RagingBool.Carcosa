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
using Epicycle.Commons.Time;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class PartyScene1 : SceneBase
    {
        private readonly LightSetup _lightSetup;
        private RythmDetector _rythmDetector;
        private RythmGenerator _rythmGenerator;
        private int _tickIndex;
        private int _subsceneId;

        private bool _isOff;

        public PartyScene1(IClock clock, LightSetup lightSetup)
        {
            _lightSetup = lightSetup;

            _rythmDetector = new RythmDetector(clock);
            _rythmDetector.OnNewBpm += OnNewBpm;

            _rythmGenerator = new RythmGenerator();
            _rythmGenerator.ResetAndSetBpm(120);
            _rythmGenerator.OnTick += OnTick;

            _tickIndex = 0;

            _subsceneId = 0;
        }

        void OnNewBpm(double newBpm)
        {
            _rythmGenerator.ResetAndSetBpm(newBpm);
        }

        void OnTick(int tickIndex)
        {
            _tickIndex = tickIndex;
        }

        public override void Enter()
        {
            _rythmGenerator.Reset();
            _isOff = false;
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
            _rythmGenerator.Update(dt);

            _lightSetup.MonoStrips[0].Intensity = (_tickIndex % 4) == 0 ? 1 : 0;

            UpdateRgbLights();
        }

        private void UpdateRgbLights()
        {
            var saturation = GetControl(3);
            var intensity = !_isOff ? (GetControl(7)) : 0.0;

            if (_subsceneId == 1)
            {
                if (_tickIndex % 16 == 0)
                {
                    intensity = 0.8;
                }
            }
            
            if (_subsceneId == 2)
            {
                if (_tickIndex % 16 == 0)
                {
                    saturation = 1;
                    intensity = 1;
                }
            }


            var hueControl0 = GetControl(0);
            var hueControl1 = GetControl(4);
            var hueControl2 = GetControl(1);
            var hueControl3 = GetControl(5);

            var rythmPhase = (_subsceneId == 0 ? _tickIndex / 4 : _tickIndex / 2) % 4;
            
            var hue0 = 0.0;
            var hue1 = 0.0;
            var hue2 = 0.0;
            var hue3 = 0.0;
            var hue4 = 0.0;
            var hue5 = 0.0;

            switch(rythmPhase)
            {
                case 0:
                    hue0 = hueControl0;
                    hue1 = hueControl1;
                    hue2 = hueControl0;
                    hue3 = hueControl1;
                    hue4 = hueControl0;
                    hue5 = hueControl1;
                    break;
                case 1:
                    hue0 = hueControl2;
                    hue1 = hueControl2;
                    hue2 = hueControl3;
                    hue3 = hueControl3;
                    hue4 = hueControl2;
                    hue5 = hueControl2;
                    break;
                case 2:
                    hue0 = hueControl1;
                    hue1 = hueControl0;
                    hue2 = hueControl1;
                    hue3 = hueControl0;
                    hue4 = hueControl1;
                    hue5 = hueControl0;
                    break;
                case 3:
                    hue0 = hueControl3;
                    hue1 = hueControl3;
                    hue2 = hueControl2;
                    hue3 = hueControl2;
                    hue4 = hueControl3;
                    hue5 = hueControl3;
                    break;
            }

            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[0], hue0, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[1], hue1, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[2], hue2, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[3], hue3, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbStrips[0], hue4, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbStrips[1], hue5, saturation, intensity);
        }

        public override void HandleSubsceneChange(int newSubscene)
        {
            _subsceneId = newSubscene;
        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
            switch(eventArgs.DrumId)
            {
                case 2:
                    _isOff = eventArgs.TriggerType == ButtonTriggerType.Pressed;
                    break;
                case 4:
                    _rythmDetector.Reset();
                    _rythmGenerator.Reset();
                    break;
                case 5:
                    _rythmDetector.Tap();
                    break;
            }
        }
    }
}
