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

using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    internal static class KeyboardControlBoardUtils
    {
        public static IDictionary<TKeyId, int> CreateKeyMapping<TKeyId>(IEnumerable<TKeyId> keys)
        {
            var mapping = new Dictionary<TKeyId, int>();

            var index = 0;
            foreach (var key in keys)
            {
                mapping[key] = index;
                index++;
            }

            return mapping;
        }
    }
}
