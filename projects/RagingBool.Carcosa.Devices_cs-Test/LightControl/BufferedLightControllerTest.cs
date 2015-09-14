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
using System.Linq;

namespace RagingBool.Carcosa.Devices.LightControl
{
    [TestFixture]
    public class BufferedLightControllerTest
    {
        private int _frameSize;
        private Mock<IFramedLightController> _lightControllerMock;
        private BufferedLightController _bufferedLightController;

        [SetUp]
        public void SetUp()
        {
            _frameSize = 5;

            _lightControllerMock = new Mock<IFramedLightController>(MockBehavior.Strict);
            _lightControllerMock.Setup(m => m.FrameSize).Returns(_frameSize);

            _bufferedLightController = new BufferedLightController(_lightControllerMock.Object);
        }

        [Test]
        public void LightController_returns_value_from_ctor()
        {
            Assert.That(_bufferedLightController.LightController, Is.SameAs(_lightControllerMock.Object));
        }

        [Test]
        public void FrameSize_same_as_FrameSize_of_the_light_controller()
        {
            Assert.That(_bufferedLightController.FrameSize, Is.EqualTo(_frameSize));
        }

        [Test]
        public void Values_zero_by_default()
        {
            for(var i = 0; i < _bufferedLightController.FrameSize; i++)
            {
                Assert.That(_bufferedLightController[i], Is.EqualTo(0));
            }
        }

        [Test]
        public void Values_setting_values_works()
        {
            _bufferedLightController[1] = 10;
            _bufferedLightController[3] = 20;
            _bufferedLightController[1] = 30;

            Assert.That(_bufferedLightController[0], Is.EqualTo(0));
            Assert.That(_bufferedLightController[1], Is.EqualTo(30));
            Assert.That(_bufferedLightController[2], Is.EqualTo(0));
            Assert.That(_bufferedLightController[3], Is.EqualTo(20));
            Assert.That(_bufferedLightController[4], Is.EqualTo(0));
        }

        [Test]
        public void GetEnumerator_returns_enumerator_that_enumerates_on_all_the_values()
        {
            var values = new byte[] { 11, 22, 33, 44, 55 };
            SetValues(values);

            Assert.That(_bufferedLightController.ToArray(), Is.EqualTo(values));
        }

        [Test]
        public void Update_passes_the_frame_state_to_the_light_controller()
        {
            var values = new byte[] { 11, 22, 33, 44, 55 };
            SetValues(values);

            _lightControllerMock.Setup(m => m.SendFrame(values)).Verifiable();
            _bufferedLightController.Update();
            _lightControllerMock.VerifyAll();
        }

        private void SetValues(byte[] values)
        {
            for (var i = 0; i < _bufferedLightController.FrameSize; i++)
            {
                _bufferedLightController[i] = values[i];
            }
        }
    }
}
