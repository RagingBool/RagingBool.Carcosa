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

using Epicycle.Commons;

namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    public sealed class FramedOpcController : IFramedLightController
    {
        private readonly IOpcController _opcController;
        private readonly int _channel;

        public FramedOpcController(IOpcController opcController, int channel)
        {
            ArgAssert.NotNull(opcController, "opcController");
            ArgAssert.InRange(channel, "channel", 0, 0xFF);

            _opcController = opcController;
            _channel = channel;
        }

        public IOpcController OpcController
        {
            get { return _opcController; }
        }

        public int Channel
        {
            get { return _channel; }
        }

        public void SendFrame(byte[] values)
        {
            _opcController.SendRgbFrame((byte) _channel, values);
        }
    }
}
