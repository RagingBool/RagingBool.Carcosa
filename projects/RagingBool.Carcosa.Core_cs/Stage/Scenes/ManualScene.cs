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
using System;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal class ManualScene : SceneBase
    {
        private readonly LightSetup _lightSetup;

        private int _monoState;
        private bool _isOff;
        private bool _isFull;

        public ManualScene(LightSetup lightSetup)
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
            UpdateRgbLights();
            UpdateMonoStrips();
        }

        private void UpdateRgbLights()
        {
            var saturation = !_isFull ? GetControl(3) : 0.0;
            var intensity = !_isOff ? (!_isFull ? GetControl(7) : 1) : 0.0;
            
            var hue0 = GetControl(0);
            var hue1 = GetControl(4);
            var hue2 = GetControl(1);
            var hue3 = GetControl(5);
            var hue4 = GetControl(2);
            var hue5 = GetControl(6);

            SetRgbLightToHsi(_lightSetup.RgbLights[0], hue0, saturation, intensity);
            SetRgbLightToHsi(_lightSetup.RgbLights[1], hue1, saturation, intensity);
            SetRgbLightToHsi(_lightSetup.RgbLights[2], hue2, saturation, intensity);
            SetRgbLightToHsi(_lightSetup.RgbLights[3], hue3, saturation, intensity);
            SetRgbLightToHsi(_lightSetup.RgbStrips[0], hue4, saturation, intensity);
            SetRgbLightToHsi(_lightSetup.RgbStrips[1], hue5, saturation, intensity);
        }

        private void UpdateMonoStrips()
        {
            if(_isOff || _isFull)
            {
                var intensity = _isOff ? 0 : 1;

                foreach(var light in _lightSetup.MonoStrips)
                {
                    light.Intensity = intensity;
                }

                return;
            }

            var intensity0 = (_monoState & 0x04) != 0 ? 1 : 0;
            var intensity1 = (_monoState & 0x02) != 0 ? 1 : 0;
            var intensity2 = (_monoState & 0x01) != 0 ? 1 : 0;

            _lightSetup.MonoStrips[0].Intensity = intensity0;
            _lightSetup.MonoStrips[1].Intensity = intensity1;
            _lightSetup.MonoStrips[2].Intensity = intensity2;
            _lightSetup.MonoStrips[3].Intensity = intensity2;
            _lightSetup.MonoStrips[4].Intensity = intensity1;
            _lightSetup.MonoStrips[5].Intensity = intensity0;
        }

        public override void HandleSubsceneChange(int newSubscene)
        {

        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
            switch (eventArgs.DrumId)
            {
                case 0:
                    _monoState = (_monoState + 1) % 8;
                    break;
                case 2:
                    _isOff = eventArgs.TriggerType == ButtonTriggerType.Pressed;
                    break;
                case 3:
                    _isFull = eventArgs.TriggerType == ButtonTriggerType.Pressed;
                    break;
            }
        }

        private void SetRgbLightToHsi(IRgbLight rgbLight, double hue, double saturation, double intensity)
        {
            // Based on a post by Brian Neltner: http://blog.saikoled.com/post/43693602826/why-every-led-light-should-be-using-hsi

            var hueRadians = 3.14159 * hue * 2.0;
            var normSaturation = saturation;
            var normIntensity = intensity;
            
            var c = normIntensity / 3;

            double redOut, greenOut, blueOut;

            // Math! Thanks in part to Kyle Miller.
            if (hueRadians < 2.09439)
            {
                var r = Math.Cos(hueRadians) / Math.Cos(1.047196667 - hueRadians);
                redOut = c * (1 + normSaturation * r);
                greenOut = c * (1 + normSaturation * (1 - r));
                blueOut = c * (1 - normSaturation);
            }
            else if (hueRadians < 4.188787)
            {
                var shiftedHue = hueRadians - 2.09439;
                var r = Math.Cos(shiftedHue) / Math.Cos(1.047196667 - shiftedHue);
                greenOut = c * (1 + normSaturation * r);
                blueOut = c * (1 + normSaturation * (1 - r));
                redOut = c * (1 - normSaturation);
            }
            else
            {
                var shiftedHue = hueRadians - 4.188787;
                var r = Math.Cos(shiftedHue) / Math.Cos(1.047196667 - shiftedHue);
                blueOut = c * (1 + normSaturation * r);
                redOut = c * (1 + normSaturation * (1 - r));
                greenOut = c * (1 - normSaturation);
            }

            rgbLight.Red = redOut;
            rgbLight.Green = greenOut;
            rgbLight.Blue = blueOut;
        }
    }
}
