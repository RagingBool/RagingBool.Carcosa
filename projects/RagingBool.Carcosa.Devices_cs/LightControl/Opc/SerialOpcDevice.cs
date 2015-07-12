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

using System.IO.Ports;

namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    public sealed class SerialOpcDevice : IOpcController, IDevice
    {
        public const int DefaultBaudRate = 38400;

        private readonly string _portName;
        private readonly int _baudRate;

        private SerialPort _serialPort;

        public SerialOpcDevice(string portName, int baudRate = DefaultBaudRate)
        {
            _portName = portName;
            _baudRate = baudRate;

            _serialPort = null;
        }

        public string PortName
        {
            get { return _portName; }
        }

        public int BaudRate
        {
            get { return _baudRate; }
        }

        public void Connect()
        {
            _serialPort = new SerialPort(_portName, _baudRate);
        }

        public void Disconnect()
        {
            _serialPort.Dispose();
            _serialPort = null;
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
            if (_serialPort != null)
            {
                _serialPort.Write(data, 0, data.Length);
            }
        }
    }
}
