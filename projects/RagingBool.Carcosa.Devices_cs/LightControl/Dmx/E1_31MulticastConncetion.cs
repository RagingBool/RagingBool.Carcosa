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

using System.Net;
using System.Net.Sockets;

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    public sealed class E1_31MulticastConncetion
    {
        public const string DefaultBaseIp = "239.255.0.0";
        public const int DefaultPort = 5568;

        private static readonly int MulticastTtl = 10;

        private readonly Socket _socket;
        private readonly IPEndPoint _ipEndPoint;

        public E1_31MulticastConncetion(int universeId, string baseIp = DefaultBaseIp, int port = DefaultPort)
        {
            IPAddress ip = GenerateIp(baseIp, universeId);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, MulticastTtl);

            _ipEndPoint = new IPEndPoint(ip, port);
        }

        private static IPAddress GenerateIp(string baseIp, int universeId)
        {
            var baseIpBytes = IPAddress.Parse(baseIp).GetAddressBytes();
            var universeIdHigh = (byte)((universeId >> 8) & 0xFF);
            var universeIdLow = (byte)(universeId & 0xFF);

            return new IPAddress(new byte[] { baseIpBytes[0], baseIpBytes[1], universeIdHigh, universeIdLow });
        }

        public void Connect()
        {
            _socket.Connect(_ipEndPoint);
        }

        public void Disconnect()
        {
            _socket.Disconnect(true);
        }

        public void SendData(byte[] data)
        {
            if(_socket.Connected)
            {
                _socket.Send(data, data.Length, SocketFlags.None);
            }
        }
    }
}
