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

using System.IO;

namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    // TODO: Use Epicycle stream when available

    /// <summary>
    /// A utilty class that can produce Open Pixel Control (OPC) protocol messages.
    /// More information about OPC can be found here: http://openpixelcontrol.org/
    /// </summary>
    public static class OpcProtocolUtils
    {
        public const byte SetPixelColorsCommand = 0x00;
        public const byte SystemExclusiveCommand = 0xFF;

        public const ushort SystemId_Fadecandy = 0x0001;

        public static byte[] BuildSetPixelColorsCommand(byte channel, byte[] rgbValues)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, channel, SetPixelColorsCommand, (ushort)rgbValues.Length);
                writer.Write(rgbValues);

                return stream.ToArray();
            }
        }

        public static byte[] BuildSystemExclusiveCommand(byte channel, ushort systemId, byte[] data)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                WriteHeader(writer, channel, SystemExclusiveCommand, (ushort)(data.Length + 2));
                WriteUint16(writer, systemId);
                writer.Write(data);

                return stream.ToArray();
            }
        }

        private static void WriteHeader(BinaryWriter writer, byte channel, byte command, ushort length)
        {
            writer.Write(channel);
            writer.Write(command);
            WriteUint16(writer, length);
        }

        private static void WriteUint16(BinaryWriter writer, ushort value)
        {
            writer.Write((byte)(value >> 8));
            writer.Write((byte)(value & 0xFF));
        }
    }
}
