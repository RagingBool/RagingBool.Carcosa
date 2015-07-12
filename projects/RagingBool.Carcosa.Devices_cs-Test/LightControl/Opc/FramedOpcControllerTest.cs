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

namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    [TestFixture]
    public class FramedOpcControllerTest
    {
        private Mock<IOpcController> _opcControllerMock;

        [SetUp]
        public void SetUp()
        {
            _opcControllerMock = new Mock<IOpcController>(MockBehavior.Strict);
        }

        [Test]
        public void FrameSize_returns_correct_value()
        {
            var frameSize = 15;

            var framedOpcController = new FramedOpcController(_opcControllerMock.Object, 123, frameSize);
            Assert.That(framedOpcController.FrameSize, Is.EqualTo(frameSize));
        }

        [Test]
        public void SendFrame_calls_parent_SendRgbFrame()
        {
            var channel = 123;
            var values = new byte[] { 1, 2, 3 };

            var framedOpcController = new FramedOpcController(_opcControllerMock.Object, channel, 15);

            _opcControllerMock.Setup(m => m.SendRgbFrame((byte)channel, values)).Verifiable();
            framedOpcController.SendFrame(values);
            _opcControllerMock.VerifyAll();
        }
    }
}
