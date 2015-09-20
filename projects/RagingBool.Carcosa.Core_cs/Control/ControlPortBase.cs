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

namespace RagingBool.Carcosa.Core.Control
{
    public abstract class ControlPortBase<TControlComponent> : IControlPort
        where TControlComponent : IControlComponent
    {
        private readonly TControlComponent _component;
        private readonly ControlPortConfiguration _configuration;

        public ControlPortBase(TControlComponent component, ControlPortConfiguration configuration)
        {
            ArgAssert.NotNull(component, "component");
            ArgAssert.NotNull(configuration, "configuration");

            _component = component;
            _configuration = configuration;
        }

        public IControlComponent Component { get { return _component; } }
        public TControlComponent TypedComponent { get { return _component; } }

        public ControlPortConfiguration Configuration { get { return _configuration; } }
    }
}
