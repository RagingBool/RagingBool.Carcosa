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

            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[0], hue0, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[1], hue1, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[2], hue2, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbLights[3], hue3, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbStrips[0], hue4, saturation, intensity);
            LightUtils.SetRgbLightToHsi(_lightSetup.RgbStrips[1], hue5, saturation, intensity);

            LightUtils.SetHueGradient(_lightSetup.DmxRgbStrips, hue0, hue1, saturation, intensity);
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
    }
}
