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

using Moq;
using NUnit.Framework;

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    [TestFixture]
    public class FramedDmxControllerTest
    {
        private Mock<IDmxUniverse> _dmxUniverseMock;

        [SetUp]
        public void SetUp()
        {
            _dmxUniverseMock = new Mock<IDmxUniverse>(MockBehavior.Strict);
        }

        [Test]
        public void FrameSize_returns_correct_value()
        {
            var frameSize = 15;

            var framedDmxController = new FramedDmxController(_dmxUniverseMock.Object, frameSize);
            Assert.That(framedDmxController.FrameSize, Is.EqualTo(frameSize));
        }

        [Test]
        public void SendFrame_calls_parent_SendFrame()
        {
            var values = new byte[] { 1, 2, 3 };

            _dmxUniverseMock.Setup(m => m.SendFrame(values)).Verifiable();

            var framedDmxController = new FramedDmxController(_dmxUniverseMock.Object);

            framedDmxController.SendFrame(values);
            _dmxUniverseMock.VerifyAll();
        }
    }
}
