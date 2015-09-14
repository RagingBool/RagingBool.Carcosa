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

using Epicycle.Commons.Collections;
using Epicycle.Math.Geometry;
using RagingBool.Carcosa.Core.Stage.Lights;
using RagingBool.Carcosa.Devices.LightControl;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class LightSetup
    {
        private readonly IBufferedLightController _dmxUniverse1;
        private readonly IBufferedLightController _snark;
        private readonly IBufferedLightController _fadecandyContoller;

        private readonly IList<IRgbLight> _rgbStrips;
        private readonly IList<IMonoLight> _monoStrips;
        private readonly IList<IRgbLight> _rgbLights;

        private readonly IList<IRgbLight> _dmxRgbStrips;

        private readonly IList<IRgbLight> _fadecandyStripAll;
        private readonly IList<IRgbLight> _fadecandyStrip1;
        private readonly IList<IRgbLight> _fadecandyStrip2;

        private readonly ILedMatrix _ledMatrix;

        public LightSetup(IBufferedLightController dmxUniverse1, IBufferedLightController snark, IBufferedLightController fadecandyContoller)
        {
            _dmxUniverse1 = dmxUniverse1 ?? new BufferedLightController(new DummyFramedLightController(10000));
            _snark = snark ?? new BufferedLightController(new DummyFramedLightController(10000));
            _fadecandyContoller = fadecandyContoller ?? new BufferedLightController(new DummyFramedLightController(10000));

            _rgbStrips = new List<IRgbLight>();
            _monoStrips = new List<IMonoLight>();
            _rgbLights = new List<IRgbLight>();

            _rgbStrips.Add(new SnarkRgbLight(_snark, 0));
            _rgbStrips.Add(new SnarkRgbLight(_snark, 3));

            for(int i = 0; i < 6; i++)
            {
                _monoStrips.Add(new SnarkMonoLight(_snark, 6 + i));
            }

            _rgbLights.Add(new DmxRgbLightSeparatedLeds(_dmxUniverse1, 0));
            _rgbLights.Add(new DmxRgbLightSeparatedLeds(_dmxUniverse1, 10));
            _rgbLights.Add(new DmxRgbLightUnifiedLeds(_dmxUniverse1, 20));
            _rgbLights.Add(new DmxRgbLightUnifiedLeds(_dmxUniverse1, 30));

            _dmxRgbStrips = new List<IRgbLight>();
            for (int i = 0; i < 9; i++)
            {
                _dmxRgbStrips.Add(new DmxSimpleRgbLight(_dmxUniverse1, i * 3));
            }

            int s1 = 32*0;
            int s2 = 32*1;
            int s3 = 32 * 2;
            int s7 = 32 * 6;
            _fadecandyStripAll = CreateFadecandyStrip(
                s1+0, s1+1, s1+2, s1+3, s1+4, // 0
                s2+0, s2+1, s2+2, s2+3, s2+4, s2+5, s2+6, s2+7, // 5
                s3+0, s3+1, s3+2, s3+3, s3+4, // 13
                s7+0, s7+1, s7+2, s7+3, s7+4 // 18
                // 23
                );

            _fadecandyStrip1 = CreateFadecandyStrip(0, 2, 4, 6, 8);
            _fadecandyStrip2 = CreateFadecandyStrip(1, 3, 5, 7);

            _ledMatrix = CreateFadecandyLedMatrix(0, new Vector2i(30, 10));
        }

        public IReadOnlyList<IRgbLight> RgbStrips
        {
            get { return _rgbStrips.AsReadOnlyList(); }
        }

        public IReadOnlyList<IMonoLight> MonoStrips
        {
            get { return _monoStrips.AsReadOnlyList(); }
        }

        public IReadOnlyList<IRgbLight> RgbLights
        {
            get { return _rgbLights.AsReadOnlyList(); }
        }

        public IReadOnlyList<IRgbLight> DmxRgbStrips
        {
            get { return _dmxRgbStrips.AsReadOnlyList(); }
        }

        public IReadOnlyList<IRgbLight> FadecandyStripAll
        {
            get { return _fadecandyStripAll.AsReadOnlyList(); }
        }

        public IReadOnlyList<IRgbLight> FadecandyStrip1
        {
            get { return _fadecandyStrip1.AsReadOnlyList(); }
        }

        public IReadOnlyList<IRgbLight> FadecandyStrip2
        {
            get { return _fadecandyStrip2.AsReadOnlyList(); }
        }

        public ILedMatrix LedMatrix
        {
            get { return _ledMatrix; }
        }

        private IList<IRgbLight> CreateFadecandyStrip(params int[] indices)
        {
            var lights = new List<IRgbLight>();

            foreach (var index in indices)
            {
                lights.Add(new FadecandyLed(_fadecandyContoller, index));
            }

            return lights;
        }

        private ILedMatrix CreateFadecandyLedMatrix(int fromChannel, Vector2i dimensions)
        {
            var pixels = new List<IRgbLight>();
            var numPixels = dimensions.X * dimensions.Y;

            for (var y = 0; y < dimensions.Y; y++)
            {
                var rowStart = y * dimensions.X;
                if(y >= 2)
                {
                    rowStart += dimensions.X * 2;
                }
                for (var x = 0; x < dimensions.X; x++)
                {
                    var shift = ((y % 2) == 0) ? x : (dimensions.X - x - 1);

                    pixels.Add(new FadecandyLed(_fadecandyContoller, fromChannel + rowStart + shift));
                }
            }

            return new LedMatrix(pixels.AsReadOnlyList(), dimensions);
        }
    }
}
