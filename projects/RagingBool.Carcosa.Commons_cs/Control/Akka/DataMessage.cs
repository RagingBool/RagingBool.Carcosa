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
    internal sealed class DataMessage
    {
        private readonly string _inputName;
        private readonly object _data;

        public DataMessage(string inputName, object data)
        {
            _inputName = inputName;
            _data = data;
        }

        public string InputName { get { return _inputName; } }
        public object Data { get { return _data; } }
    }
}
