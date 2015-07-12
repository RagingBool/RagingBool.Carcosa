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

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    public sealed class FramedDmxController : IFramedLightController
    {
        public const int DmxDefaultFrameSize = 512;

        private readonly IDmxUniverse _dmxUniverse;
        private readonly int _frameSize;

        public FramedDmxController(IDmxUniverse dmxUniverse, int frameSize = DmxDefaultFrameSize)
        {
            ArgAssert.NotNull(dmxUniverse, "dmxUniverse");
            ArgAssert.AtLeast(frameSize, "frameSize", 0);

            _dmxUniverse = dmxUniverse;
            _frameSize = frameSize;
        }

        public IDmxUniverse DmxUniverse
        {
            get { return _dmxUniverse; }
        }

        public int FrameSize
        {
            get { return _frameSize; }
        }

        public void SendFrame(byte[] values)
        {
            _dmxUniverse.SendFrame(values);
        }
    }
}
