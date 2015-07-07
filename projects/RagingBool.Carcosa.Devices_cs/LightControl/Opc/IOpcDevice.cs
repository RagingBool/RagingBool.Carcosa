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


namespace RagingBool.Carcosa.Devices.LightControl.Opc
{
    /// <summary>
    /// A lighting control device that communicates using the Open Pixel Control (OPC) protocol.
    /// More information about OPC can be found here: http://openpixelcontrol.org/
    /// </summary>
    public interface IOpcDevice : IDevice
    {
        /// <summary>
        /// Send a frame.
        /// </summary>
        /// <param name="channel">The channel to use.</param>
        /// <param name="rgbValues">The RGB data to use. Each 3 bytes are assumed to be a 24-bit RGB color.</param>
        void SendRgbFrame(byte channel, byte[] rgbValues);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">The channel to use.</param>
        /// <param name="systemId">a 16-bit ID of the intended system.</param>
        /// <param name="commandData">The sytem-specific command.</param>
        void SendSystemExclusive(byte channel, ushort systemId, byte[] commandData);
    }
}
