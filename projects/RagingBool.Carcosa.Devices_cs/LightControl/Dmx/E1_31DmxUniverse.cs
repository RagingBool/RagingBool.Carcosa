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

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    public sealed class E1_31DmxUniverse : IDmxUniverse
    {
        private readonly int _universeId;
        private readonly Guid _componentIdentifier;
        private readonly string _sourceName;

        private readonly E1_31MulticastConncetion _connection;

        public E1_31DmxUniverse(
            int universeId, 
            Guid componentIdentifier, 
            string sourceName,
            string baseIp = E1_31MulticastConncetion.DefaultBaseIp,
            int port = E1_31MulticastConncetion.DefaultPort)
        {
            _universeId = universeId;
            _componentIdentifier = componentIdentifier;
            _sourceName = sourceName;

            _connection = new E1_31MulticastConncetion(universeId, baseIp, port);
        }

        public void Connect()
        {
            _connection.Connect();
        }

        public void Disconnect()
        {
            _connection.Disconnect();
        }

        public void SendFrame(byte[] values)
        {
            var packet = E1_31ProtocolUtils.CreatePacket(_componentIdentifier, _sourceName, _universeId, values);

            _connection.SendData(packet);
        }
    }
}
