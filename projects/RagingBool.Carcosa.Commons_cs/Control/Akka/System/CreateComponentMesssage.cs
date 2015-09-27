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

namespace RagingBool.Carcosa.Commons.Control.Akka.System
{
    public sealed class CreateComponentMesssage : IControlNetworkMessage
    {
        private readonly string _name;
        private readonly Type _componentType;
        private readonly object _configuration;

        public CreateComponentMesssage(string name, Type componentType, object configuration)
        {
            _name = name;
            _componentType = componentType;
            _configuration = configuration;
        }

        public string Name { get { return _name; } }
        public Type ComponentType { get { return _componentType; } }
        public object Configuration { get { return _configuration; } }
    }
}
