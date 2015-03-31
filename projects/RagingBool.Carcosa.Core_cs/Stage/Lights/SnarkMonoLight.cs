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

using RagingBool.Carcosa.Devices;

namespace RagingBool.Carcosa.Core.Stage.Lights
{
    internal sealed class SnarkMonoLight : IMonoLight
    {
        private readonly ISnark _snark;
        private readonly int _channel;

        private double _intensity;

        public SnarkMonoLight(ISnark snark, int channel)
        {
            _snark = snark;
            _channel = channel;

            _intensity = 0;
        }

        public double Intensity
        {
            get { return _intensity; }
            set
            {
                _intensity = value;
                Update();
            }
        }

        private void Update()
        {
            _snark.SetChannel(_channel, _intensity.UnitToByte());
        }
    }
}
