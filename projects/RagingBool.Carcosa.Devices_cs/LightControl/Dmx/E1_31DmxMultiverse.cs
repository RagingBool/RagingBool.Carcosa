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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.LightControl.Dmx
{
    public sealed class E1_31DmxMultiverse : IDevice, IDmxMultiverse
    {
        private readonly IDictionary<int, E1_31DmxUniverse> _universes;

        public E1_31DmxMultiverse(
            IEnumerable<int> universeIds,
            Guid componentIdentifier,
            string sourceName,
            string baseIp = E1_31MulticastConncetion.DefaultBaseIp,
            int port = E1_31MulticastConncetion.DefaultPort)
        {
            ArgAssert.NotNull(universeIds, "universeIds");
            ArgAssert.NotNull(componentIdentifier, "componentIdentifier");
            ArgAssert.NotNull(sourceName, "sourceName");
            ArgAssert.NotNull(baseIp, "baseIp");

            _universes = new Dictionary<int, E1_31DmxUniverse>();

            foreach(var universeId in universeIds)
            {
                if(_universes.ContainsKey(universeId))
                {
                    throw new ArgumentException(string.Format("Duplicate universe ID (%d)!", universeId));
                }

                _universes[universeId] = new E1_31DmxUniverse(universeId, componentIdentifier, sourceName, baseIp, port);
            }
        }

        public void Connect()
        {
            foreach(var universe in _universes.Values)
            {
                universe.Connect();
            }
        }
        
        public void Disconnect()
        {
            foreach (var universe in _universes.Values)
            {
                universe.Disconnect();
            }
        }

        public void Update()
        {
            // Nothing to do...
        }

        public IDmxUniverse getUniverse(int universeId)
        {
            if (!_universes.ContainsKey(universeId))
            {
                throw new ArgumentException(string.Format("No universe with ID %d", universeId));
            }

            return _universes[universeId];
        }
    }
}
