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

using RagingBool.Carcosa.Devices;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal sealed class LightDrumPad : Button
    {
        private int _lightTtl;
        private bool _forceOn;

        public LightDrumPad(ILpd8 controller, int buttonId, bool isContinues)
            : base(controller, buttonId, isContinues ? ButtonTriggerBehaviour.Continues : ButtonTriggerBehaviour.OnPush)
        {
            _forceOn = false;
            _lightTtl = 0;
        }

        public void HandleTrigger(ButtonTriggerType triggerType)
        {
            switch(triggerType)
            {
                case ButtonTriggerType.Trigger:
                    _lightTtl = 2;
                    break;
                case ButtonTriggerType.Pressed:
                    _forceOn = true;
                    break;
                case ButtonTriggerType.Released:
                    _forceOn = false;
                    break;
            }
        }

        protected override bool? RenderFrame(int phases)
        {
            if(!_forceOn && _lightTtl <= 0)
            {
                return false;
            }

            _lightTtl--;

            return true;
        }
    }
}
