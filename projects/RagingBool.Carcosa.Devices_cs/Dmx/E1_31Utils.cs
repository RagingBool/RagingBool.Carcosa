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

using System;
using System.Linq;
using System.Text;

namespace RagingBool.Carcosa.Devices.Dmx
{
    internal static class E1_31Utils
    {
        public static byte[] CreateDmxPacket(int universeId, byte[] values)
        {
            var data = CreateDMPLayer(values);
            data = FramingLayer(data, (short)universeId);
            data = RootLayer(data);

            return data;
        }

        private static byte[] CreateDMPLayer(byte[] data)
        {
            var arr = new byte[11 + data.Length];
            //packet length
            Tuple<byte, byte> size_bytes = length_as_low12(10 + 1 + data.Length);
            arr[0] = size_bytes.Item1;
            arr[1] = size_bytes.Item2;
            // vector
            arr[2] = 0x02;
            // # address type & data type
            arr[3] = 0xa1;
            // # startcode
            arr[4] = 0x00;
            arr[5] = 0x00;
            //increment value
            arr[6] = 0x00;
            arr[7] = 0x01;
            ushort number = Convert.ToUInt16(1 + data.Length);
            byte upper = (byte)(number >> 8);
            byte lower = (byte)(number & 0xff);
            arr[8] = upper;
            arr[9] = lower;
            // DMX 512 startcode
            arr[10] = 0x00;
            //DMX 512 data
            var byte_data = new byte[data.Length];
            byte_data = data.Select(x => (byte)x).ToArray();
            Array.Copy(byte_data, 0, arr, 11, data.Length);
            // packet.extend(self.data)
            return arr;
        }

        private static byte[] FramingLayer(byte[] data, short universe, short priority = 100, uint sequence = 0, string name = "lumos")
        {
            var packet = new byte[77 + data.Length];
            Tuple<byte, byte> size_bytes = length_as_low12(77 + data.Length);
            packet[0] = size_bytes.Item1;
            packet[1] = size_bytes.Item2;
            //vector
            packet[2] = 0x00;
            packet[3] = 0x00;
            packet[4] = 0x00;
            packet[5] = 0x02;
            byte[] name_bytes = Encoding.ASCII.GetBytes(name);
            System.Buffer.BlockCopy(name_bytes, 0, packet, 6, name_bytes.Length);
            //the length of the name is 64 according to the protocol s
            int arr_counter = 6 + 64;
            packet[arr_counter] = (byte)priority;
            arr_counter += 1;
            packet[arr_counter] = 0x00;
            arr_counter += 1;
            packet[arr_counter] = 0x00;
            arr_counter += 1;
            packet[arr_counter] = (byte)sequence;
            //options
            arr_counter += 1;
            packet[arr_counter] = 0;
            //universe
            arr_counter += 1;
            byte[] byte_universe = BitConverter.GetBytes(universe);
            packet[arr_counter] = byte_universe[1];
            arr_counter += 1;
            packet[arr_counter] = byte_universe[0];
            arr_counter += 1;
            System.Buffer.BlockCopy(data, 0, packet, arr_counter, data.Length);
            return packet;

        }

        private static byte[] RootLayer(byte[] data)
        {
            Guid uuid1 = Guid.NewGuid();
            // pdu size starts after byte 16 - there are 38 bytes of data in root layer
            // so size is 38 - 16 + framing layer 
            var packet = new byte[38 + data.Length];

            System.Buffer.BlockCopy("\x00\x10\x00\x00".ToCharArray(), 0, packet, 0, 4);
            Array.Reverse(packet, 0, 4);
            int counter = 4;
            int str_length = "ASC-E1.17\x00\x00\x00".Length;
            byte[] toBytes = Encoding.ASCII.GetBytes("ASC-E1.17\x00\x00\x00");
            System.Buffer.BlockCopy(toBytes, 0, packet, 4, str_length);
            counter += str_length;
            //pdu size starts after byte 16 - there are 38 bytes of data in root layer
            // so size is 38 - 16 + framing layer
            Tuple<byte, byte> size_bytes = length_as_low12(38 - 16 + data.Length);
            packet[counter] = size_bytes.Item1;
            counter++;
            packet[counter] = size_bytes.Item2;
            counter++;
            //vector
            toBytes = Encoding.ASCII.GetBytes("\x00\x00\x00\x04");
            System.Buffer.BlockCopy(toBytes, 0, packet, counter, 4);
            counter = counter + 4;
            System.Buffer.BlockCopy(uuid1.ToByteArray(), 0, packet, counter, 16);
            counter += 16;
            System.Buffer.BlockCopy(data, 0, packet, counter, data.Length);
            return packet;
        }

        private static Tuple<byte, byte> length_as_low12(int i)
        {
            ushort number = Convert.ToUInt16(0x7000 | i);
            byte upper = (byte)(number >> 8);
            byte lower = (byte)(number & 0xff);
            return Tuple.Create(upper, lower);
        }
    }
}
