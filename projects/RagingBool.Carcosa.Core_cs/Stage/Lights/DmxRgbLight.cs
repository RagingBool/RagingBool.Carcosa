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

using RagingBool.Carcosa.Devices.Dmx;

namespace RagingBool.Carcosa.Core.Stage.Lights
{
    internal sealed class DmxRgbLight : IRgbLight
    {
        private readonly IDmxMultiverse _dmxMultiverse;
        private readonly int _universeId;
        private readonly int _channelRed;
        private readonly int _channelGreen;
        private readonly int _channelBlue;

        private double _red;
        private double _green;
        private double _blue;

        public DmxRgbLight(IDmxMultiverse dmxMultiverse, int universeId, int channelRed, int channelGreen, int channelBlue)
        {
            _dmxMultiverse = dmxMultiverse;
            _universeId = universeId;
            _channelRed = channelRed;
            _channelGreen = channelGreen;
            _channelBlue = channelBlue;

            _red = 0;
            _green = 0;
            _blue = 0;
        }

        public double Red
        {
            get { return _red; }
            set
            {
                _red = value;
                Update();
            }
        }

        public double Green
        {
            get { return _green; }
            set
            {
                _green = value;
                Update();
            }
        }

        public double Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                Update();
            }
        }

        private void Update()
        {
            _dmxMultiverse.SetChannel(_universeId, _channelRed, _red.UnitToByte());
            _dmxMultiverse.SetChannel(_universeId, _channelGreen, _green.UnitToByte());
            _dmxMultiverse.SetChannel(_universeId, _channelBlue, _blue.UnitToByte());
        }
    }
}
