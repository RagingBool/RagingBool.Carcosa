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

using System.Net.Sockets;

namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    // TODO: Test

    public sealed class OpcDevice : IOpcController, IDevice
    {
        private readonly string _host;
        private readonly int _port;
        private TcpClient _tcpClient;

        public OpcDevice(string host, int port)
        {
            _host = host;
            _port = port;

            _tcpClient = null;
        }

        public void Connect()
        {
            if (_tcpClient == null)
            {
                _tcpClient = new TcpClient(_host, _port);
                _tcpClient.NoDelay = true;
            }
        }

        public void Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
            }
        }
    
        public void SendRgbFrame(byte channel, byte[] rgbValues)
        {
            SendData(OpcProtocolUtils.BuildSetPixelColorsCommand(channel, rgbValues));
        }

        public void SendSystemExclusive(byte channel, ushort systemId, byte[] commandData)
        {
            SendData(OpcProtocolUtils.BuildSystemExclusiveCommand(channel, systemId, commandData));
        }

        private void SendData(byte[] data)
        {
            if (_tcpClient != null)
            {
                _tcpClient.GetStream().Write(data, 0, data.Length);
            }
        }
    }
}
