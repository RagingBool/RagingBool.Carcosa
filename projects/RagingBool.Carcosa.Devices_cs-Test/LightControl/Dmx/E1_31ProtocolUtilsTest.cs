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
using System;
using System.IO;
using System.Text;

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    [TestFixture]
    public class E1_31ProtocolUtilsTest
    {
        // TODO: Use Epicycle streams when available

        [Test]
        public void CreatePacket_creates_proper_packet()
        {
            var cid = Guid.NewGuid();
            var sourceName = "test";
            var universeId = 123;
            var dmxData = new byte[512];

            for(var i = 0; i < dmxData.Length; i++)
            {
                dmxData[i] = (byte)((i * 13) % 0xFF);
            }

            var packet = E1_31ProtocolUtils.CreatePacket(cid, sourceName, universeId, dmxData);
            var expected = BuildExpectedPacket(cid, sourceName, universeId, dmxData);

            Assert.That(packet, Is.EqualTo(expected));
        }

        private static byte[] BuildExpectedPacket(Guid cid, string sourceName, int universeId, byte[] dmxData)
        {
            const int RootLayerHeaderSize = 22;
            const int FramingLayerHeaderSize = 77;
            const int DmpLayerHeaderSize = 10;

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                //// Root Layer

                // Preamble Size & Post-amble Size
                writer.Write(new byte[] { 0x00, 0x10, 0x00, 0x00 });

                // ACN Packet Identifier
                WriteStringUtf8(writer, "ASC-E1.17", 12);

                // Flags and Length
                WriteFlagsAndLength(writer, RootLayerHeaderSize + FramingLayerHeaderSize + DmpLayerHeaderSize + dmxData.Length + 1);

                // Vector
                writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x04 });

                // CID
                writer.Write(cid.ToByteArray());

                //// E1.31 Framing Layer

                // Flags and Length
                WriteFlagsAndLength(writer, FramingLayerHeaderSize + DmpLayerHeaderSize + dmxData.Length + 1);

                // Vector
                writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x02 });

                // Source Name
                WriteStringUtf8(writer, sourceName, 64);

                // Priority
                writer.Write((byte)100);

                // Reserved
                writer.Write(new byte[] { 0x00, 0x00 });

                // Sequence Number
                writer.Write((byte)0x00);

                // Options
                writer.Write((byte)0x00);

                // Universe
                WriteUint16(writer, (ushort)universeId);

                //// DMP Layer

                // Flags and Length
                WriteFlagsAndLength(writer, DmpLayerHeaderSize + dmxData.Length + 1);

                // Vector
                writer.Write((byte)0x02);

                // Address Type & Data Type
                writer.Write((byte)0xA1);

                // First Property Address & Address Increment
                writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x01 });

                // Property value count
                WriteUint16(writer, (ushort)(dmxData.Length + 1));

                // Property values
                writer.Write((byte)0x00); // START Code
                writer.Write(dmxData);

                return stream.ToArray();
            }
        }

        private static void WriteFlagsAndLength(BinaryWriter writer, int length)
        {
            WriteUint16(writer, (ushort)(0x7000 | (ushort)length));
        }

        private static void WriteUint16(BinaryWriter writer, ushort value)
        {
            writer.Write((byte)(value >> 8));
            writer.Write((byte)(value & 0xFF));
        }

        private static void WriteStringUtf8(BinaryWriter writer, string s, int fixedSize)
        {
            var data = Encoding.UTF8.GetBytes(s);
            writer.Write(data);

            var padding = fixedSize - data.Length; 
            for(var i = 0; i < padding; i++)
            {
                writer.Write((byte)0x00);
            }
        }
    }
}
