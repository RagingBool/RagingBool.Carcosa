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

using Epicycle.Commons.Time;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.Dmx
{
    public sealed class E1_31DmxMultiverse : IDmxMultiverse
    {
        private readonly IClock _clock;
        private readonly double _fps;
        private readonly IDictionary<int, DmxUniverse> _universes;
        private double _lastUpdateTime;

        public E1_31DmxMultiverse(IClock clock, double fps)
        {
            _clock = clock;
            _fps = fps;
            _universes = new Dictionary<int, DmxUniverse>();
        }

        public void Connect()
        {
            _lastUpdateTime = _clock.Time;
        }

        public void Disconnect()
        {

        }

        public void AddUniverse(int universeId)
        {
            var universe = new DmxUniverse(universeId);

            _universes[universeId] = universe;
        }

        public void SetChannel(int universeId, int channelId, int value)
        {
            _universes[universeId].SetChannel(channelId, value);
        }

        public void Update()
        {
            var curTime = _clock.Time;
            var timeSinceLastUpdate = curTime - _lastUpdateTime;

            if (timeSinceLastUpdate < (1 / _fps))
            {
                return;
            }

            SendFrame();

            _lastUpdateTime = curTime;
        }

        private void SendFrame()
        {
            foreach (var universe in _universes.Values)
            {
                universe.Update();
            }
        }

        private sealed class DmxUniverse
        {
            private readonly int _universeId;
            
            private readonly E1_31Conncetion _connection;
            private readonly byte[] _values;

            public DmxUniverse(int universeId)
            {
                _universeId = universeId;

                _connection = new E1_31Conncetion(universeId);
                _values = new byte[512];

                Clear();
            }

            public void Clear()
            {
                for(int i = 0; i < _values.Length; i++)
                {
                    _values[i] = 0;
                }
            }

            public void SetChannel(int channelId, int value)
            {
                _values[channelId] = (byte)value;
            }

            public void Update()
            {
                var packetData = E1_31Utils.CreateDmxPacket(_universeId, _values);

                _connection.SendData(packetData);
            }
        }
    }
}
