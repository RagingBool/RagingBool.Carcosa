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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public abstract class ControllerBoardBase<TControllerId, TControllerValue> : IControllerBoard<TControllerId, TControllerValue>
    {
        private readonly TControllerValue _defaultValue;
        private readonly Dictionary<TControllerId, TControllerValue> _values;

        public ControllerBoardBase(TControllerValue defaultValue)
        {
            _defaultValue = defaultValue;

            _values = new Dictionary<TControllerId, TControllerValue>();
        }

        protected void ProcessControllerChangeEvent(ControllerChangeEventArgs<TControllerId, TControllerValue> eventArgs)
        {
            var id = eventArgs.ControllerId;
            var value = eventArgs.Value;

            if (_values.ContainsKey(id) && _values[id].Equals(value))
            {
                return;
            }

            _values[id] = value;

            if (OnControllerChangeEvent != null)
            {
                OnControllerChangeEvent(this, eventArgs);
            }
        }

        public event EventHandler<ControllerChangeEventArgs<TControllerId, TControllerValue>> OnControllerChangeEvent;

        public TControllerValue GetControllerValue(TControllerId controllerId)
        {
            if(!_values.ContainsKey(controllerId))
            {
                return _defaultValue;
            }

            return _values[controllerId];
        }
    }
}
