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

using Epicycle.Input.Controllers;
using System;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public sealed class DummyControllerBoard<TControllerId, TControllerValue> : IControllerBoard<TControllerId, TControllerValue>
    {
        private readonly TControllerValue _defaultValue;

        public DummyControllerBoard(TControllerValue defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public event EventHandler<ControllerChangeEventArgs<TControllerId, TControllerValue>> OnControllerChangeEvent { add { } remove { } }

        public TControllerValue GetControllerValue(TControllerId controllerId)
        {
            return _defaultValue;
        }
    }
}
