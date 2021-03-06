﻿// [[[[INFO>
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

using Epicycle.Commons;
using Epicycle.Commons.Time;
using Epicycle.Math.Geometry;
using RagingBool.Carcosa.Core.Stage.Controller;
using RagingBool.Carcosa.Core.Stage.Scenes.Signal;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class PartyScene1 : SceneBase
    {
        private readonly LightSetup _lightSetup;
        private RythmDetector _rythmDetector;
        private RythmGenerator _rythmGenerator;
        private int _tickIndex;
        private int _subsceneId;
        private Oscillator _oscillator;
        private LedMatrixTestPattern _testPattern;

        private bool _isOff;
        private bool _strobe;

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

            _oscillator = new Oscillator();
            _oscillator.Function = Oscillator.FunctionType.SawUp;
            _oscillator.Frequency = 0.2;

            _testPattern = new LedMatrixTestPattern(_lightSetup.LedMatrix);
        }

        void OnNewBpm(double newBpm)
        {
            _rythmGenerator.ResetAndSetBpm(newBpm);
        }

        void OnTick(int tickIndex)
        {
            _testPattern.Step();
            _tickIndex = tickIndex;
        }

        public override void Enter()
        {
            _rythmGenerator.Reset();
            _isOff = false;
            _strobe = false;
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
            _oscillator.Update(dt);

            _rythmGenerator.Update(dt);

            _lightSetup.MonoStrips[0].Intensity = (_tickIndex % 4) == 0 ? 1 : 0;

            UpdateRgbLights(dt);
        }

        private void UpdateRgbLights(double dt)
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

            var gradHue = (hue0 + hue1) / 2;
            var gradHueOpening = System.Math.Abs(hue3 - hue4);
            //LightUtils.SetHueGradientAround(_lightSetup.FadecandyStripAll, gradHue, gradHueOpening, saturation, intensity);

            //RenderGradient(hueControl0, hueControl1, saturation, intensity);

            //_testPattern.Hue0 = hueControl0;
            //_testPattern.Hue1 = hueControl1;
            //_testPattern.Hue2 = hueControl2;
            //_testPattern.Hue3 = hueControl3;
            _testPattern.Hue0 = 0;
            _testPattern.Hue1 = 0.25;
            _testPattern.Hue2 = 0.5;
            _testPattern.Hue3 = 0.75;
            _testPattern.Saturation = saturation;
            _testPattern.Intensity = intensity;
            _testPattern.Strobe = _strobe;

            _testPattern.Update(dt);
        }

        private void RenderGradient(double hue1, double hue2, double saturation, double intensity)
        {
            var ledMatrix = _lightSetup.LedMatrix;
            var dimensions = ledMatrix.Dimensions;

            var center = ((Vector2) dimensions) / 2.0;
            var r = System.Math.Max(dimensions.X, dimensions.Y) / 3.0;

            //center += new Vector2(_oscillator.Value * 4, _oscillator.Value * 4);

            var pixel = ledMatrix[13, 2];
            pixel.Red = hue1;
            pixel.Green = hue2;
            pixel.Blue = 0;

            for(var x = 0; x < dimensions.X; x++)
            {
                for(var y = 0; y < dimensions.Y; y++)
                {
                    var point = new Vector2(x, y);
                    var fade = BasicMath.Clip((point - center).Norm / r, 0, 1);
                    var hue = BasicMath.Interpolate(fade, hue1, hue2);

                    LightUtils.SetRgbLightToHsi(ledMatrix[x, y], hue, saturation, intensity);
                    //ledMatrix[x, y].Red = 1;
                    //ledMatrix[x, y].Green = 1;
                    //ledMatrix[x, y].Blue = 1;
                }
            }
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
                case 3:
                    _strobe = eventArgs.TriggerType == ButtonTriggerType.Pressed;
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
