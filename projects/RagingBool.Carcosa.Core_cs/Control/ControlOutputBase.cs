﻿// [[[[INFO>
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

namespace RagingBool.Carcosa.Core.Control
{
    public abstract class ControlOutputBase : ControlPortBase, IControlOutput
    {
        public ControlOutputBase(IControlComponent component, string name, Type dataType)
            : base(component, name, dataType) { }

        public bool CanConnectTo(IControlInput input)
        {
            ArgAssert.NotNull(input, "input");

            // TODO: Check for circles in the graph

            return input.DataType.IsAssignableFrom(DataType);
        }

        public void ConnectTo(IControlInput input)
        {
            ArgAssert.NotNull(input, "input");

            PerformConnection(input);
        }

        protected abstract void PerformConnection(IControlInput input);
    }
}
