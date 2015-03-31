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
using System.IO.Ports;
using System.Text;

namespace RagingBool.Carcosa.Devices
{
    public sealed class SerialSnark : ISnark
    {
        private readonly IClock _clock;
        private readonly double _fps;

        private SerialPort _serialPort;

        private readonly int[] _state;
        private double _lastUpdateTime;

        public SerialSnark(IClock clock, string portName, int numChannels, double fps)
        {
            _clock = clock;
            _fps = fps;

            _state = new int[numChannels];

            _serialPort = new SerialPort(portName, 38400);
        }

        public int NumChannels
        {
            get { return _state.Length; }
        }

        public void SetChannel(int id, int value)
        {
            _state[id] = value;
        }

        public void Connect()
        {
            _serialPort.Open();

            _lastUpdateTime = _clock.Time;
        }

        public void Disconnect()
        {
            _serialPort.Close();
        }

        public void Update()
        {
            var curTime = _clock.Time;
            var timeSinceLastUpdate = curTime - _lastUpdateTime;

            if(timeSinceLastUpdate < (1 / _fps))
            {
                return;
            }

            SendFrame();

            _lastUpdateTime = curTime;
        }

        private void SendFrame()
        {
            var messageData = new StringBuilder();

            messageData.Append("S");
            bool isFirst = true;
            foreach (var value in _state)
            {
                if(!isFirst)
                {
                    messageData.AppendFormat(" ");
                }
                isFirst = false;

                messageData.AppendFormat("{0:X2}", value);
            }
            messageData.Append("!");

            _serialPort.Write(messageData.ToString());
        }
    }
}
