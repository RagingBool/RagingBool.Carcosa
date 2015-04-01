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
using System.Net;
using System.Net.Sockets;

namespace RagingBool.Carcosa.Devices.Dmx
{
    internal sealed class E1_31Conncetion
    {
        private Socket _socket;

        public E1_31Conncetion(int universeId)
        {
            int ttl = 5;
            int port = 5568;
            int high_byte = (universeId >> 8) & 0xff;
            int low_byte = universeId & 0xff;
            String ip_str = "239.255." + high_byte.ToString() + "." + low_byte.ToString();

            IPAddress ip = IPAddress.Parse(ip_str);
            _socket = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP,
                        SocketOptionName.AddMembership, new MulticastOption(ip));
            _socket.SetSocketOption(SocketOptionLevel.IP,
                        SocketOptionName.MulticastTimeToLive, ttl);

            IPEndPoint ipep = new IPEndPoint(ip, port);
            _socket.Connect(ipep);
        }

        public void SendData(byte[] data)
        {
            _socket.Send(data, data.Length, SocketFlags.None);
        }
    }
}
