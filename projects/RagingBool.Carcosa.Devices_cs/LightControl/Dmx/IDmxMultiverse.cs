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

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    /// <summary>
    /// A device that controls several indexed DMX512 universes. It only allows sending DMX messages.
    /// More information about the protocol can be found here: https://en.wikipedia.org/wiki/DMX512
    /// </summary>
    public interface IDmxMultiverse : IDevice
    {
        /// <summary>
        /// Sends a whole frame of DMX512 data
        /// </summary>
        /// <param name="universe">The index of the DMX512 universe.</param>
        /// <param name="values">The DMX channel data. Each byte represents a channel. Expected to be exactly 512.</param>
        void SendFrame(int universe, byte[] values);
    }
}
