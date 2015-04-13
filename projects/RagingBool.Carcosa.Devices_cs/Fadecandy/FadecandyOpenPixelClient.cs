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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RagingBool.Carcosa.Devices.Fadecandy
{
    public sealed class FadecandyOpenPixelClient
    {
        private readonly string _host;
        private readonly int _port;

        private Socket _socket;

        private TcpClient _tcpClient;

        private byte[] _data;

        public FadecandyOpenPixelClient(string host, int port, int chanels)
        {
            _host = host;
            _port = port;

            _tcpClient = new TcpClient("host", port);

            /*
            IPAddress ip = IPAddress.Parse(host);
            _socket = new Socket(AddressFamily.InterNetwork, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(ip, port);
            _socket.Connect(ipep);*/

            _data = new byte[chanels];
        }

        public void SetChannel(int channel, byte value)
        {
            _data[channel] = value;
        }

        public void Update()
        {
            var packet = OpenPixelUtils.buildPacket(0, _data);

            _tcpClient.GetStream().Write(packet, 0, packet.Length);
        }
    }
}
