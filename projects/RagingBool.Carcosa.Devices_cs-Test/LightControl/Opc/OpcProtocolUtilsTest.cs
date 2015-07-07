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

using NUnit.Framework;

namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    [TestFixture]
    public class OpcProtocolUtilsTest
    {
        private byte[] data;

        [SetUp]
        public void SetUp()
        {
            data = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
        }

        [Test]
        public void BuildSetPixelColorsCommand_creates_correect_packet()
        {
            var command = OpcProtocolUtils.BuildSetPixelColorsCommand(0x95, new byte[] { 0x10, 0x20, 0x30 });

            Assert.That(command, Is.EqualTo(new byte[] { 0x95, 0x00, 0x00, 0x03, 0x10, 0x20, 0x30 }));
        }

        [Test]
        public void BuildSystemExclusiveCommand_creates_correect_packet()
        {
            var command = OpcProtocolUtils.BuildSystemExclusiveCommand(0x95, 0xABCD, new byte[] { 0x10, 0x20, 0x30 });

            Assert.That(command, Is.EqualTo(new byte[] { 0x95, 0xFF, 0x00, 0x05, 0xAB, 0xCD, 0x10, 0x20, 0x30 }));
        }
    }
}
