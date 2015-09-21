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

namespace RagingBool.Carcosa.Commons.Control.Akka
{
    public sealed class ConnectToMessage
    {
        private readonly string _localOutput;
        private readonly string _inputId;

        public ConnectToMessage(string localOutput, string inputId)
        {
            _localOutput = localOutput;
            _inputId = inputId;
        }

        public string LocalOutput { get { return _localOutput; } }
        public string InputId { get { return _inputId; } }
    }
}
