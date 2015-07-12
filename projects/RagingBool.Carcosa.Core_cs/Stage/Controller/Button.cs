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

using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.Lpd8;
using System;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal class Button
    {
        private readonly ILpd8 _controller;
        private readonly int _buttonId;
        private readonly ButtonTriggerBehaviour _triggerBehaviour;

        private int _phase;
        private int _savedVelocity;

        public Button(ILpd8 controller, int buttonId, ButtonTriggerBehaviour triggerBehaviour)
        {
            _controller = controller;
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
                _controller.SetKeyLightState(_buttonId, isOn.Value);
            }

            _phase = (_phase + 1) % 256;
        }

        public void ProcessButtonEventHandler(ButtonEventArgs eventArgs)
        {
            if (eventArgs.ButtonId != _buttonId)
            {
                return;
            }

            int velocity = eventArgs.Velocity;

            switch (_triggerBehaviour)
            {
                case ButtonTriggerBehaviour.OnPush:
                    if (eventArgs.ButtonEventType == ButtonEventType.Pressed)
                    {
                        FireEvent(ButtonTriggerType.Trigger, velocity);
                    }
                    break;
                case ButtonTriggerBehaviour.OnRelease:
                    if (eventArgs.ButtonEventType == ButtonEventType.Released)
                    {
                        FireEvent(ButtonTriggerType.Trigger, velocity);
                    }
                    break;
                case ButtonTriggerBehaviour.Continues:
                    if (eventArgs.ButtonEventType == ButtonEventType.Pressed)
                    {
                        _savedVelocity = velocity;
                        FireEvent(ButtonTriggerType.Pressed, velocity);
                    }
                    else if (eventArgs.ButtonEventType == ButtonEventType.Released)
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
