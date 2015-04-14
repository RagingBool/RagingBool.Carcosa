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

using Epicycle.Commons.Time;
using System;
using System.Net.Sockets;

namespace RagingBool.Carcosa.Devices.Fadecandy
{
    public sealed class FadecandyOpenPixelClient
    {
        private readonly string _host;
        private readonly int _port;
        private readonly IClock _clock;
        private readonly double _fps;
        private double _lastUpdateTime;

        private TcpClient _tcpClient;

        private byte[] _data;

        public FadecandyOpenPixelClient(IClock clock, string host, int port, int chanels, double fps)
        {
            _host = host;
            _port = port;
            _clock = clock;
            _fps = fps;

            _tcpClient = new TcpClient(host, port);
            _tcpClient.NoDelay = true;

            _data = new byte[chanels];

            _lastUpdateTime = _clock.Time;
        }

        public void SetChannel(int channel, byte value)
        {
            _data[channel] = value;
        }

        public void Update()
        {
            var curTime = _clock.Time;
            var timeSinceLastUpdate = curTime - _lastUpdateTime;

            if (timeSinceLastUpdate < (1 / _fps))
            {
                return;
            }

            var packet = OpenPixelUtils.buildPacket(0, _data);

            for (var i = 0; i < packet.Length; i++)
            {
                Console.Write("{0}, ", packet[i]);
            }
            Console.WriteLine();

            _tcpClient.GetStream().Write(packet, 0, packet.Length);

            _lastUpdateTime = curTime;
        }
    }
}
