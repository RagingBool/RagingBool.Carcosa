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
using System;
using System.IO;
using System.Text;

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    // TODO: Use Epicycle streams when available

    /// <summary>
    /// Implementation of the E1.31 (DMX512 over ACN) protocol.
    /// More info can be found here: http://tsp.plasa.org/tsp/documents/published_docs.php
    /// </summary>
    public static class E1_31ProtocolUtils
    {
        private const int PropertyValuesLengthOverhead = 1;
        private const int DmpLayerLengthOverhead = PropertyValuesLengthOverhead + 10;
        private const int E1_31FramingLayerLengthOverhead = DmpLayerLengthOverhead + 77;
        private const int RootLayerLengthOverhead = E1_31FramingLayerLengthOverhead + 22;

        public const byte DefaulPriority = 100;

        private static readonly byte[] RootLayer_Prefix = new byte[]
        {
            0x00, 0x10, // Preamble Size
            0x00, 0x00, // Post-amble Size
            0x41, 0x53, 0x43, 0x2D, 0x45, 0x31, 0x2E, 0x31, 0x37, 0x00, 0x00, 0x00, // ACN Packet Identifier
        };

        private static readonly byte[] RootLayer_Vector = new byte[]
        {
            0x00, 0x00, 0x00, 0x04
        };

        private static readonly byte[] E1_31FramingLayer_Vector = new byte[]
        {
            0x00, 0x00, 0x00, 0x02
        };

        private static readonly byte[] DmpLayer_Fields = new byte[]
        {
            0x02,       // Vector
            0xA1,       // Address Type & Data Type
            0x00, 0x00, // First Property Address 
            0x00, 0x01  // Address Increment
        };

        /// <summary>
        /// Generates a E1.31 packet according to the specification.
        /// </summary>
        /// <param name="componentIdentifier">A unique GUID that identifies this device</param>
        /// <param name="sourceName">A text ID of the current device</param>
        /// <param name="universeId">The universe ID (must be 0 to 65535, note that 0 and 64000-65535 are reserved)</param>
        /// <param name="propeties">The DMX values (length must be between 0 and 512)</param>
        /// <returns></returns>
        public static byte[] CreatePacket(Guid componentIdentifier, string sourceName, int universeId, byte[] propeties)
        {
            ArgAssert.NotNull(componentIdentifier, "componentIdentifier");
            ArgAssert.NotNull(sourceName, "sourceName");
            ArgAssert.InRange(universeId, "universeId", 0, 65535);
            ArgAssert.NotNull(propeties, "propeties");
            ArgAssert.InRange(propeties.Length, "propeties.Length", 0, 512);

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var propetiesLength = propeties.Length;

                WriteRootLayerHeader(writer, propetiesLength, componentIdentifier);
                WriteE1_31FramingLayerHeader(writer, propetiesLength, sourceName, universeId);
                WriteDmpLayerHeader(writer, propetiesLength);
                WriteProperties(writer, propeties);

                return stream.ToArray();
            }
        }

        private static void WriteRootLayerHeader(BinaryWriter writer, int propertiesLength, Guid componentIdentifier)
        {
            // Preamble Size; Post-amble Size; ACN Packet Identifier
            writer.Write(RootLayer_Prefix);

            // Flags and Length
            WriteFlagsAndLength(writer, propertiesLength + RootLayerLengthOverhead);

            // Vector
            writer.Write(RootLayer_Vector);

            // CID (Component Identifier)
            writer.Write(componentIdentifier.ToByteArray());
        }

        private static void WriteE1_31FramingLayerHeader(BinaryWriter writer, int propertiesLength, string sourceName, int universeId)
        {
            // Flags and Length
            WriteFlagsAndLength(writer, propertiesLength + E1_31FramingLayerLengthOverhead);

            // Vector
            writer.Write(E1_31FramingLayer_Vector);

            // Source Name
            WriteStringUtf8FixedSize(writer, sourceName, 64);

            // Priority
            writer.Write((byte)DefaulPriority);
            
            // Reserved
            writer.Write((byte)0);
            writer.Write((byte)0);

            // Sequence Number
            writer.Write((byte)0); // TODO: Understand this better

            // Options
            writer.Write((byte)0); // Preview_Data = FALSE; Stream_Terminated = FALSE

            // Universe
            WriteUint16(writer, (ushort)universeId);
        }

        private static void WriteDmpLayerHeader(BinaryWriter writer, int propertiesLength)
        {
            // Flags and Length
            WriteFlagsAndLength(writer, propertiesLength + DmpLayerLengthOverhead);

            // Vector; Address Type & Data Type; First Property Address; Address Increment
            writer.Write(DmpLayer_Fields);

            // Property value count
            WriteUint16(writer, (ushort)(propertiesLength + PropertyValuesLengthOverhead));
        }

        private static void WriteProperties(BinaryWriter writer, byte[] properties)
        {
            // DMX512-A START Code
            writer.Write((byte)0);

            // Data
            writer.Write(properties);
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

        private static void WriteStringUtf8FixedSize(BinaryWriter writer, string s, int fixedSize)
        {
            var data = Encoding.UTF8.GetBytes(s);
            writer.Write(data);

            var padding = fixedSize - data.Length;
            for (var i = 0; i < padding; i++)
            {
                writer.Write((byte)0x00);
            }
        }
    }
}
