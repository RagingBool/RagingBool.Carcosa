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

using Epicycle.Commons.Time;
using RagingBool.Carcosa.Devices;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal sealed class LiveControllerMode : ControllerModeBase
    {
        private const int SceneSelectButton = 7;
        private const int SubsceneSelectButton = 3;

        private const int LightDrumButton0 = 1;
        private const int LightDrumButton1 = 2;
        private const int LightDrumButton2 = 5;
        private const int LightDrumButton3 = 6;

        private readonly List<Button> _buttons;
        private readonly List<LightDrumPad> _lightDrumPads;

        private readonly Button _sceneSelectorButton;
        private readonly SubsceneSelectorButton _subsceneSelectorButton;

        private int _subsceneId;

        public LiveControllerMode(ControllerUi controllerUi, IClock clock, ILpd8 controller)
            : base(controllerUi, clock, controller)
        {
            _buttons = new List<Button>();
            _lightDrumPads = new List<LightDrumPad>();

            _sceneSelectorButton = new Button(Controller, SceneSelectButton, ButtonTriggerBehaviour.OnRelease);
            _sceneSelectorButton.OnTrigger += HandleSceneSelect;
            _buttons.Add(_sceneSelectorButton);

            _subsceneSelectorButton = new SubsceneSelectorButton(Controller, SubsceneSelectButton);
            _subsceneSelectorButton.OnTrigger += HandleSubsceneSelect;
            _buttons.Add(_subsceneSelectorButton);

            AddLightDrumButton(LightDrumButton0, false);
            AddLightDrumButton(LightDrumButton1, false);
            AddLightDrumButton(LightDrumButton2, true);
            AddLightDrumButton(LightDrumButton3, true);

            SubsceneId = 0;
        }

        public int SubsceneId
        {
            get { return _subsceneId; }

            set
            {
                if(_subsceneId == value)
                {
                    return;
                }

                _subsceneId = value;
                _subsceneSelectorButton.SubsceneId = _subsceneId;

                ControllerUi.FireOnSceneChange();
            }
        }

        private void AddLightDrumButton(int buttonId, bool isContinues)
        {
            int lightDrumIndex = _lightDrumPads.Count;

            var button = new LightDrumPad(Controller, buttonId, isContinues);
            button.OnTrigger += (sender, eventArgs) => { HandleLightDrumTrigger(lightDrumIndex, eventArgs); };
            _buttons.Add(button);
            _lightDrumPads.Add(button);
        }

        public override void Enter()
        {
            Fps = 60;
        }

        protected override void NewFrame()
        {
            base.NewFrame();

            foreach (var button in _buttons)
            {
                button.NewFrame();
            }
        }

        public override void ProcessButtonEventHandler(ButtonEventArgs eventArgs)
        {
            foreach (var button in _buttons)
            {
                button.ProcessButtonEventHandler(eventArgs);
            }
        }

        private void HandleSceneSelect(object sender, ButtonTriggerEventArgs eventArgs)
        {
            ControllerUi.GoToSceneSelectMode();
        }

        private void HandleSubsceneSelect(object sender, ButtonTriggerEventArgs eventArgs)
        {
            SubsceneId = (_subsceneId + 1) % 3;
        }

        private void HandleLightDrumTrigger(int lightDrumIndex, ButtonTriggerEventArgs eventArgs)
        {
            _lightDrumPads[lightDrumIndex].HandleTrigger(eventArgs.TriggerType);
            ControllerUi.FireLightDrumEvent(new LightDrumEventArgs(lightDrumIndex, eventArgs.TriggerType, eventArgs.Velocity));
        }

        public override void ProcessControllerChangeEvent(ControllerChangeEventArgs eventArgs)
        {
            ControllerUi.FireControlParameterValueChange(new ControlParameterValueChangeEventArgs(eventArgs.ControllerId, eventArgs.Value));
        }
    }
}
