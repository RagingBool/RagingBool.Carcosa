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
using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RagingBool.Carcosa.Devices.InputControl.ControlBoard
{
    public sealed class KeyboardControlBoard<TKeyId> : IControlBoard
    {
        private const int MinControllerValue = 0;
        private const int MaxControllerValue = 255;

        private readonly IKeyboard<TKeyId, TimedKey> _keyboardManager;
        private readonly VelocityKeyboardEmulator<int, TKeyId> _buttonsKeyboard;

        private readonly List<Controller> _controllers;

        private readonly Dictionary<TKeyId, double> _absoluteFaderKeys;
        private readonly Dictionary<TKeyId, double> _relativeFaderKeys;

        public KeyboardControlBoard(
            IKeyboard<TKeyId, TimedKey> keyboardManager,
            IEnumerable<TKeyId> buttonKeys,
            int defaultVelocity, int highVelocity,
            TKeyId highVelocityKey,
            IEnumerable<TKeyId> controllerKeys,
            IEnumerable<TKeyId> faderKeys,
            TKeyId faderUpKey, TKeyId faderDownKey, TKeyId faderFastUpKey, TKeyId faderFastDownKey,
            double faderSmallStepResolution, double faderBigStepResolution)
        {
            _keyboardManager = keyboardManager;

            // Init buttons

            _buttonsKeyboard = new VelocityKeyboardEmulator<int, TKeyId>(
                keyboardManager,
                CreateButtonKeyMapping(buttonKeys),
                defaultVelocity, highVelocity,
                highVelocityKey
                );

            // Init controllers

            _controllers = new List<Controller>();
            foreach (var controllerKey in controllerKeys)
            {
                _controllers.Add(new Controller(this, _controllers.Count, controllerKey));
            }

            _absoluteFaderKeys = new Dictionary<TKeyId, double>();
            PopulateAbsoluteFaderKeys(faderKeys);

            _relativeFaderKeys = new Dictionary<TKeyId, double>();
            PopulateRelativeFaderKeys(
                faderUpKey, faderDownKey, faderFastUpKey, faderFastDownKey,
                faderSmallStepResolution, faderBigStepResolution);

            // Register key events

            _keyboardManager.OnKeyEvent += OnKeyEvent;
        }

        private static IDictionary<TKeyId, int> CreateButtonKeyMapping(IEnumerable<TKeyId> buttonKeys)
        {
            var mapping = new Dictionary<TKeyId, int>();

            var index = 0;
            foreach(var key in buttonKeys)
            {
                mapping[key] = index;
                index++;
            }

            return mapping;
        }

        private void PopulateAbsoluteFaderKeys(IEnumerable<TKeyId> faderKeys)
        {
            var low = (double)MinControllerValue;
            var high = (double)MaxControllerValue;

            var count = faderKeys.Count();

            if (count <= 0)
            {
                return;
            }

            if (count == 1)
            {
                throw new ArgumentException("There can not be one fader key");
            }

            var step = (high - low) / (faderKeys.Count() - 1);

            var curValue = low;

            foreach (var key in faderKeys)
            {
                _absoluteFaderKeys[key] = curValue;

                curValue += step;
            }
        }

        private void PopulateRelativeFaderKeys(
            TKeyId faderUpKey, TKeyId faderDownKey, TKeyId faderFastUpKey, TKeyId faderFastDownKey,
            double faderSmallStepResolution, double faderBigStepResolution)
        {
            var range = MaxControllerValue - MinControllerValue;
            var smallStep = range / faderSmallStepResolution;
            var bigStep = range / faderBigStepResolution;

            _relativeFaderKeys[faderUpKey] = smallStep;
            _relativeFaderKeys[faderDownKey] = -smallStep;
            _relativeFaderKeys[faderFastUpKey] = bigStep;
            _relativeFaderKeys[faderFastDownKey] = -bigStep;
        }

        private void OnKeyEvent(object sender, KeyEventArgs<TKeyId, TimedKey> e)
        {
            foreach (var controller in _controllers)
            {
                controller.OnKeyEvent(e);
            }
        }

        public int NumberOfButtons { get { return 0; } }
        public int NumberOfControllers { get { return 0; } }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return _buttonsKeyboard; } }

        public event EventHandler<ControllerChangeEventArgs<int, int>> OnControllerChange;

        public void SetKeyLightState(int id, bool newState)
        {
            // We don't have lights on the keyboard :(
        }

        private class Controller
        {
            private readonly KeyboardControlBoard<TKeyId> _parent;
            private readonly int _controlId; 
            private readonly TKeyId _keyId;
            private double _curValue;
            private int _lastSentValue;
            private bool _isArmed;

            public Controller(KeyboardControlBoard<TKeyId> parent, int controlId, TKeyId keyId)
            {
                _parent = parent;
                _controlId = controlId;
                _keyId = keyId;

                _lastSentValue = MinControllerValue - 1;
                _curValue = MinControllerValue;
                _isArmed = false;
            }

            public void OnKeyEvent(KeyEventArgs<TKeyId, TimedKey> e)
            {
                var key = e.KeyId;
                var eventType = e.EventType;

                if (key.Equals(_keyId))
                {
                    if (eventType != KeyEventType.Repeat)
                    {
                        _isArmed = eventType == KeyEventType.Pressed;
                    }
                }
                else if (_isArmed)
                {
                    if (eventType == KeyEventType.Pressed && _parent._absoluteFaderKeys.ContainsKey(key))
                    {
                        SetNewValue(_parent._absoluteFaderKeys[key]);
                    }
                    else if ((eventType == KeyEventType.Pressed || eventType == KeyEventType.Repeat) && _parent._relativeFaderKeys.ContainsKey(key))
                    {
                        SetNewValue(_curValue + _parent._relativeFaderKeys[key]);
                    }
                }
            }

            private void SetNewValue(double newValue)
            {
                _curValue = BasicMath.Clip(newValue, MinControllerValue, MaxControllerValue);
                var clippedRoundedValue = BasicMath.Clip(BasicMath.Round(newValue), MinControllerValue, MaxControllerValue);

                if (clippedRoundedValue == _lastSentValue)
                {
                    return;
                }

                _lastSentValue = clippedRoundedValue;

                if (_parent.OnControllerChange != null)
                {
                    _parent.OnControllerChange(_parent, new ControllerChangeEventArgs<int, int>(_controlId, _lastSentValue));
                }
            }
        }
    }
}
