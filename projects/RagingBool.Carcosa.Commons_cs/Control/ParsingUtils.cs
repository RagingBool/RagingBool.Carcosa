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
using System.Text.RegularExpressions;

namespace RagingBool.Carcosa.Commons.Control
{
    public static class ParsingUtils
    {
        public static void ParseControlInputId(string id, out string controlId, out string inputName)
        {
            ParseControlPortId(id, "in", out controlId, out inputName);
        }

        public static void ParseControlOutputId(string id, out string controlId, out string outputName)
        {
            ParseControlPortId(id, "out", out controlId, out outputName);
        }

        public static void ParseControlPortId(string id, string expectedType, out string controlId, out string portName)
        {
            var regex = new Regex(@"^([\w\.]*):(\w+)(?:.(\w+))?$");

            var match = regex.Match(id);

            if(!match.Success)
            {
                throw new ArgumentException("Illegal port id!");
            }

            var type = match.Groups[2].Value;

            if (type != expectedType)
            {
                throw new ArgumentException("Illegal port type!");
            }

            controlId = match.Groups[1].Value;
            portName = match.Groups[3].Value;
        }
    }
}
