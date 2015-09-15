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
using Epicycle.Input.Keyboard;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public abstract class KeyboardControllerBoardEmulator<TControllerId, TControllerValue, TKeyId, TAdditionalKeyEventData>
        : ControllerBoardBase<TControllerId, TControllerValue>
    {
        private readonly IKeyboard<TKeyId, TAdditionalKeyEventData> _baseKeyboard;
        private readonly IDictionary<TKeyId, TControllerId> _controllerKeyMapping;
        private readonly IDictionary<TKeyId, TControllerValue> _valueKeysMapping;
        private readonly TKeyId _smallAdvanceForwardKey;
        private readonly TKeyId _smallAdvanceBackwardKey;
        private readonly TKeyId _bigAdvanceForwardKey;
        private readonly TKeyId _bigAdvanceBackwardKey;

        private readonly IDictionary<TControllerId, bool> _armedControllers;

        public KeyboardControllerBoardEmulator(
            IKeyboard<TKeyId, TAdditionalKeyEventData> baseKeyboard, 
            TControllerValue defaultValue,
            IDictionary<TKeyId, TControllerId> controllerKeyMapping,
            IDictionary<TKeyId, TControllerValue> valueKeysMapping,
            TKeyId smallAdvanceForwardKey, TKeyId smallAdvanceBackwardKey,
            TKeyId bigAdvanceForwardKey, TKeyId bigAdvanceBackwardKey)

            : base(defaultValue)
        {
            _baseKeyboard = baseKeyboard;
            _controllerKeyMapping = controllerKeyMapping;
            _valueKeysMapping = valueKeysMapping;
            _smallAdvanceForwardKey = smallAdvanceForwardKey;
            _smallAdvanceBackwardKey = smallAdvanceBackwardKey;
            _bigAdvanceForwardKey = bigAdvanceForwardKey;
            _bigAdvanceBackwardKey = bigAdvanceBackwardKey;

            _armedControllers = new Dictionary<TControllerId, bool>();
            foreach(var controllerId in _controllerKeyMapping.Values)
            {
                _armedControllers[controllerId] = false;
            }

            _baseKeyboard.OnKeyEvent += OnKeyEventHandler;
        }

        private void OnKeyEventHandler(object sender, KeyEventArgs<TKeyId, TAdditionalKeyEventData> e)
        {
            var key = e.KeyId;
            var eventType = e.EventType;

            if (_controllerKeyMapping.ContainsKey(key))
            {
                if (eventType != KeyEventType.Repeat)
                {
                    var id = _controllerKeyMapping[key];

                    _armedControllers[id] = eventType == KeyEventType.Pressed;
                }
            }
            else if (_valueKeysMapping.ContainsKey(key))
            {
                if (eventType == KeyEventType.Pressed)
                {
                    SetValueForArmedControllers(_valueKeysMapping[key]);
                }
            }
            else if (eventType == KeyEventType.Pressed || eventType == KeyEventType.Repeat)
            {
                if (key.Equals(_smallAdvanceForwardKey))
                {
                    AdvanceValueForArmedControllers(true, false);
                }
                else if (key.Equals(_smallAdvanceBackwardKey))
                {
                    AdvanceValueForArmedControllers(false, false);
                }
                else if (key.Equals(_bigAdvanceForwardKey))
                {
                    AdvanceValueForArmedControllers(true, true);
                }
                else if (key.Equals(_bigAdvanceBackwardKey))
                {
                    AdvanceValueForArmedControllers(false, true);
                }
            }
        }

        private void SetValueForArmedControllers(TControllerValue value)
        {
            foreach (var entry in _armedControllers)
            {
                if (entry.Value)
                {
                    var eventArgs = new ControllerChangeEventArgs<TControllerId, TControllerValue>(entry.Key, value);
                    ProcessControllerChangeEvent(eventArgs);
                }
            }
        }

        private void AdvanceValueForArmedControllers(bool isForwardOrBackward, bool isBigOrSmall)
        {
            foreach (var entry in _armedControllers)
            {
                if (entry.Value)
                {
                    var id = entry.Key;

                    var oldValue = GetControllerValue(id);
                    var newValue = AdvanceValue(oldValue, isForwardOrBackward, isBigOrSmall);

                    var eventArgs = new ControllerChangeEventArgs<TControllerId, TControllerValue>(entry.Key, newValue);
                    ProcessControllerChangeEvent(eventArgs);
                }
            }
        }

        protected abstract TControllerValue AdvanceValue(TControllerValue value, bool isForwardOrBackward, bool isBigOrSmall);
    }
}
