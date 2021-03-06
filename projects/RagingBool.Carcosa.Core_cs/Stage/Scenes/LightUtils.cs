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

using RagingBool.Carcosa.Core.Stage.Lights;
using System;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal static class LightUtils
    {
        public static void SetHueGradientAround(IReadOnlyList<IRgbLight> lights, double midHue, double hueOpening, double saturation, double intensity)
        {
            var maxOpening = 0.5;
            var deltaHue = hueOpening / 2 * maxOpening;
            var hue1 = midHue - deltaHue;
            var hue2 = midHue + deltaHue;

            SetHueGradient(lights, hue1, hue2, saturation, intensity);
        }

        public static void SetHueGradient(IReadOnlyList<IRgbLight> lights, double hue1, double hue2, double saturation, double intensity)
        {
            var hueStep = (hue2 - hue1) / (lights.Count - 1);
            for (int i = 0; i < lights.Count; i++)
            {
                LightUtils.SetRgbLightToHsi(lights[i], hue1 + hueStep * i, saturation, intensity);
            }
        }

        public static void SetRgbLightToHsi(IRgbLight rgbLight, double hue, double saturation, double intensity)
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
