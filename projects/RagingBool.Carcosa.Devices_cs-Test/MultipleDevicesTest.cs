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

namespace RagingBool.Carcosa.Devices
{
    [TestFixture]
    public class MultipleDevicesTest
    {
        private Mock<IDevice> _deviceMock1;
        private Mock<IDevice> _deviceMock2;
        private Mock<IDevice> _deviceMock3;

        private MultipleDevices _multipleDevices;

        [SetUp]
        public void SetUp()
        {
            _deviceMock1 = new Mock<IDevice>();
            _deviceMock2 = new Mock<IDevice>();
            _deviceMock3 = new Mock<IDevice>();

            _multipleDevices = new MultipleDevices(new IDevice[] { 
                _deviceMock1.Object,
                _deviceMock2.Object,
                _deviceMock3.Object});
        }

        [Test]
        public void Connect_gets_delegated_to_all_the_children()
        {
            _multipleDevices.Connect();

            _deviceMock1.Verify(m => m.Connect(), Times.Once);
            _deviceMock2.Verify(m => m.Connect(), Times.Once);
            _deviceMock3.Verify(m => m.Connect(), Times.Once);
        }

        [Test]
        public void Disconnect_gets_delegated_to_all_the_children()
        {
            _multipleDevices.Disconnect();

            _deviceMock1.Verify(m => m.Disconnect(), Times.Once);
            _deviceMock2.Verify(m => m.Disconnect(), Times.Once);
            _deviceMock3.Verify(m => m.Disconnect(), Times.Once);
        }
    }
}
