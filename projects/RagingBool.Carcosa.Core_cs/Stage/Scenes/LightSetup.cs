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
using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.Dmx;
using RagingBool.Carcosa.Devices.Fadecandy;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class LightSetup
    {
        private readonly IDmxMultiverse _dmxMultiverse;
        private readonly ISnark _snark;
        private readonly FadecandyOpenPixelClient _fadecandyClient;

        private readonly IList<IRgbLight> _rgbStrips;
        private readonly IList<IMonoLight> _monoStrips;
        private readonly IList<IRgbLight> _rgbLights;

        private readonly IList<IRgbLight> _dmxRgbStrips;

        private readonly IList<IRgbLight> _fadecandyStripAll;
        private readonly IList<IRgbLight> _fadecandyStrip1;
        private readonly IList<IRgbLight> _fadecandyStrip2;

        private readonly ILedMatrix _ledMatrix;
        
        public LightSetup(IDmxMultiverse dmxMultiverse, ISnark snark, FadecandyOpenPixelClient fadecandyClient)
        {
            _dmxMultiverse = dmxMultiverse;
            _snark = snark;
            _fadecandyClient = fadecandyClient;

            _rgbStrips = new List<IRgbLight>();
            _monoStrips = new List<IMonoLight>();
            _rgbLights = new List<IRgbLight>();

            _rgbStrips.Add(new SnarkRgbLight(_snark, 0));
            _rgbStrips.Add(new SnarkRgbLight(_snark, 3));

            for(int i = 0; i < 6; i++)
            {
                _monoStrips.Add(new SnarkMonoLight(_snark, 6 + i));
            }

            _rgbLights.Add(new DmxRgbLightSeparatedLeds(_dmxMultiverse, 1, 0));
            _rgbLights.Add(new DmxRgbLightSeparatedLeds(_dmxMultiverse, 1, 10));
            _rgbLights.Add(new DmxRgbLightUnifiedLeds(_dmxMultiverse, 1, 20));
            _rgbLights.Add(new DmxRgbLightUnifiedLeds(_dmxMultiverse, 1, 30));

            _dmxRgbStrips = new List<IRgbLight>();
            for (int i = 0; i < 9; i++)
            {
                _dmxRgbStrips.Add(new DmxSimpleRgbLight(_dmxMultiverse, 1, i * 3));
            }

            _fadecandyStripAll = CreateFadecandyStrip(0, 1, 2, 3, 4, 5, 6, 7, 8);
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
                lights.Add(new FadecandyLed(_fadecandyClient, index));
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

                    pixels.Add(new FadecandyLed(_fadecandyClient, fromChannel + rowStart + shift));
                }
            }

            return new LedMatrix(pixels.AsReadOnlyList(), dimensions);
        }
    }
}
