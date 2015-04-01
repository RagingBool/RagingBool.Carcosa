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
using RagingBool.Carcosa.Core.Stage.Lights;
using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.Dmx;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class LightSetup
    {
        private readonly IDmxMultiverse _dmxMultiverse;
        private readonly ISnark _snark;

        private readonly IList<IRgbLight> _rgbStrips;
        private readonly IList<IMonoLight> _monoStrips;
        private readonly IList<IRgbLight> _rgbLights;

        public LightSetup(IDmxMultiverse dmxMultiverse, ISnark snark)
        {
            _dmxMultiverse = dmxMultiverse;
            _snark = snark;

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
    }
}
