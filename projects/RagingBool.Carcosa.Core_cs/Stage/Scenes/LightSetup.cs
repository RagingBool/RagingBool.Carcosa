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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class LightSetup
    {
        private readonly ISnark _snark;

        private readonly IList<IRgbLight> _rgbStrips;
        private readonly IList<IMonoLight> _monoStrips;

        public LightSetup(ISnark snark)
        {
            _snark = snark;

            _rgbStrips = new List<IRgbLight>();
            _monoStrips = new List<IMonoLight>();

            _rgbStrips.Add(new SnarkRgbLight(_snark, 0));
            _rgbStrips.Add(new SnarkRgbLight(_snark, 3));

            for(int i = 0; i < 6; i++)
            {
                _monoStrips.Add(new SnarkMonoLight(_snark, 6 + i));
            }
        }

        public IReadOnlyList<IRgbLight> RgbStrips
        {
            get { return _rgbStrips.AsReadOnlyList(); }
        }

        public IReadOnlyList<IMonoLight> MonoStrips
        {
            get { return _monoStrips.AsReadOnlyList(); }
        }
    }
}
