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

using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal class Button
    {
        private readonly IControlBoard _controlBoard;
        private readonly int _buttonId;
        private readonly ButtonTriggerBehaviour _triggerBehaviour;

        private int _phase;
        private int _savedVelocity;

        public Button(IControlBoard controlBoard, int buttonId, ButtonTriggerBehaviour triggerBehaviour)
        {
            _controlBoard = controlBoard;
            _buttonId = buttonId;
            _triggerBehaviour = triggerBehaviour;

            _phase = 0;
            _savedVelocity = 0;
        }

        public event EventHandler<ButtonTriggerEventArgs> OnTrigger;

        public void NewFrame()
        {
            var isOn = RenderFrame(_phase);

            if(isOn.HasValue)
            {
                _controlBoard.ButtonLights.SetIndicatorValue(_buttonId, isOn.Value);
            }

            _phase = (_phase + 1) % 256;
        }

        public void ProcessButtonEventHandler(KeyEventArgs<int, TimedKeyVelocity> eventArgs)
        {
            if (eventArgs.KeyId != _buttonId)
            {
                return;
            }

            int velocity = eventArgs.AdditionalData.Velocity;

            switch (_triggerBehaviour)
            {
                case ButtonTriggerBehaviour.OnPush:
                    if (eventArgs.EventType == KeyEventType.Pressed)
                    {
                        FireEvent(ButtonTriggerType.Trigger, velocity);
                    }
                    break;
                case ButtonTriggerBehaviour.OnRelease:
                    if (eventArgs.EventType == KeyEventType.Released)
                    {
                        FireEvent(ButtonTriggerType.Trigger, velocity);
                    }
                    break;
                case ButtonTriggerBehaviour.Continues:
                    if (eventArgs.EventType == KeyEventType.Pressed)
                    {
                        _savedVelocity = velocity;
                        FireEvent(ButtonTriggerType.Pressed, velocity);
                    }
                    else if (eventArgs.EventType == KeyEventType.Released)
                    {
                        FireEvent(ButtonTriggerType.Released, _savedVelocity);
                    }
                    break;
            }
        }

        private void FireEvent(ButtonTriggerType triggerType, int velocity)
        {
            if(OnTrigger != null)
            {
                OnTrigger(this, new ButtonTriggerEventArgs(_buttonId, triggerType, velocity));
            }
        }

        protected virtual bool? RenderFrame(int phase)
        {
            return null;
        }
    }
}
