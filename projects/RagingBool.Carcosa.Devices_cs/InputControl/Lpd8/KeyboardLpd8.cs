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

namespace RagingBool.Carcosa.Devices.InputControl.Lpd8
{
    public sealed class KeyboardLpd8<TKeyId> : ILpd8
    {
        private const int MinControllerValue = 0;
        private const int MaxControllerValue = 255;

        private readonly IKeyboard<TKeyId, TimedKey> _keyboardManager;
        private readonly int _defaultVelocity;
        private readonly int _highVelocity;
        private readonly TKeyId _highVelocityKey;

        private readonly List<Button> _buttons;
        private readonly List<Controller> _controllers;

        private readonly Dictionary<TKeyId, double> _absoluteFaderKeys;
        private readonly Dictionary<TKeyId, double> _relativeFaderKeys;

        public KeyboardLpd8(
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

            _buttons = new List<Button>();
            foreach (var buttonKey in buttonKeys)
            {
                _buttons.Add(new Button(this, _buttons.Count, buttonKey));
            }
            _defaultVelocity = defaultVelocity;
            _highVelocity = highVelocity;
            _highVelocityKey = highVelocityKey;

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
            foreach (var button in _buttons)
            {
                button.OnKeyEvent(e);
            }

            foreach (var controller in _controllers)
            {
                controller.OnKeyEvent(e);
            }
        }

        public int NumberOfButtons { get { return 0; } }
        public int NumberOfControllers { get { return 0; } }

        public event EventHandler<KeyEventArgs<int, TimedKeyVelocity>> OnButtonEvent;
        public event EventHandler<ControllerChangeEventArgs<int, int>> OnControllerChange;

        public void SetKeyLightState(int id, bool newState)
        {
            // We don't have lights on the keyboard :(
        }

        private abstract class ControlBase
        {
            private readonly KeyboardLpd8<TKeyId> _parent;
            private readonly int _controlId;

            public ControlBase(KeyboardLpd8<TKeyId> parent, int controlId)
            {
                _parent = parent;
                _controlId = controlId;
            }

            protected KeyboardLpd8<TKeyId> Parent { get { return _parent; } }

            protected int ControlId { get { return _controlId; } }

            abstract public void OnKeyEvent(KeyEventArgs<TKeyId, TimedKey> e);
        }

        private class Button : ControlBase
        {
            private readonly TKeyId _keyId;
            private int _pressVelocity;

            public Button(KeyboardLpd8<TKeyId> parent, int controlId, TKeyId keyId)
                : base(parent, controlId)
            {
                _keyId = keyId;
            }

            public override void OnKeyEvent(KeyEventArgs<TKeyId, TimedKey> e)
            {
                if (e.EventType != KeyEventType.Repeat && e.KeyId.Equals(_keyId))
                {
                    if (Parent.OnButtonEvent != null)
                    {
                        var eventType = e.EventType;

                        int velocity;
                        if (eventType == KeyEventType.Pressed)
                        {
                            var isHighVelocity = Parent._keyboardManager.GetKeyState(Parent._highVelocityKey) == KeyState.Pressed;
                            velocity = isHighVelocity ? Parent._highVelocity : Parent._defaultVelocity;
                            _pressVelocity = velocity;
                        }
                        else
                        {
                            velocity = _pressVelocity;
                        }

                        var timedKeyVelocity = new TimedKeyVelocity(e.AdditionalData.Time, velocity);
                        var outEvent = new KeyEventArgs<int, TimedKeyVelocity>(ControlId, eventType, timedKeyVelocity);

                        Parent.OnButtonEvent(Parent, outEvent);
                    }
                }
            }
        }

        private class Controller : ControlBase
        {
            private readonly TKeyId _keyId;
            private double _curValue;
            private int _lastSentValue;
            private bool _isArmed;

            public Controller(KeyboardLpd8<TKeyId> parent, int controlId, TKeyId keyId)
                : base(parent, controlId)
            {
                _keyId = keyId;

                _lastSentValue = MinControllerValue - 1;
                _curValue = MinControllerValue;
                _isArmed = false;
            }

            public override void OnKeyEvent(KeyEventArgs<TKeyId, TimedKey> e)
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
                    if (eventType == KeyEventType.Pressed && Parent._absoluteFaderKeys.ContainsKey(key))
                    {
                        SetNewValue(Parent._absoluteFaderKeys[key]);
                    }
                    else if ((eventType == KeyEventType.Pressed || eventType == KeyEventType.Repeat) && Parent._relativeFaderKeys.ContainsKey(key))
                    {
                        SetNewValue(_curValue + Parent._relativeFaderKeys[key]);
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

                if (Parent.OnControllerChange != null)
                {
                    Parent.OnControllerChange(Parent, new ControllerChangeEventArgs<int, int>(ControlId, _lastSentValue));
                }
            }
        }
    }
}
