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
using System.Collections.Generic;
using System.Linq;

namespace RagingBool.Carcosa.Devices
{
    public sealed class MultipleDevices : IDevice
    {        
        private readonly IList<IDevice> _devices;

        public MultipleDevices(IEnumerable<IDevice> devices)
        {
            ArgAssert.NotNull(devices, "devices");

            _devices = devices.ToList();
        }

        public void Connect()
        {
            foreach (var device in _devices)
            {
                device.Connect();
            }
        }

        public void Disconnect()
        {
            foreach (var device in _devices)
            {
                device.Disconnect();
            }
        }
    }
}
