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

using Akka.Actor;
using RagingBool.Carcosa.Commons;
using RagingBool.Carcosa.Commons.Control.Akka.System;
using RagingBool.Carcosa.Devices.InputControl;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    internal sealed class CarcosaControlActor : UntypedActor
    {
        private readonly ControlSystemActorRef _controlSystemActor;
        private readonly IList<string> _startStopControls;

        public CarcosaControlActor()
        {
            _controlSystemActor = new ControlSystemActorRef(Context.ActorOf<ControlSystemActor>("system"));
            _startStopControls = new List<string>();

            _controlSystemActor.CreateComponent(
                "controllerUi",
                typeof(ControllerUiActor),
                Unit.Instance);
            _startStopControls.Add("controllerUi");
        }

        protected override void OnReceive(object message)
        {
            if (message is RegisterWindowsKeyboardMessage)
            {
                OnRegisterWindowsKeyboardMessage((RegisterWindowsKeyboardMessage)message);
            }
            else if (message is StartMessage)
            {
                OnStartStop(message);
            }
            else if (message is StopMessage)
            {
                OnStartStop(message);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void OnStartStop(object message)
        {
            foreach (var control in _startStopControls)
            {
                _controlSystemActor.SendMessage(control, message);
            }
        }
        
        private void OnRegisterWindowsKeyboardMessage(RegisterWindowsKeyboardMessage message)
        {
            _controlSystemActor.CreateComponent(
                "keyboard",
                typeof(ExternalKeyboardActor<WindowsKey, TimedKey>),
                message.Keyboard);

            if(message.KeyboardControlBoardConfig != null)
            {
                _controlSystemActor.CreateComponent(
                    "keyboardControlBoardButtons",
                    typeof(KeyboardControlBoardButtonsActor<WindowsKey>),
                    message.KeyboardControlBoardConfig.ButtonsConfig);

                _controlSystemActor.CreateComponent(
                    "keyboardControlBoardControllers",
                    typeof(KeyboardControlBoardControllersActor<WindowsKey>),
                    message.KeyboardControlBoardConfig.ControllersConfig);

                _controlSystemActor.Connect("keyboard:out", "keyboardControlBoardButtons:in");
                _controlSystemActor.Connect("keyboard:out", "keyboardControlBoardControllers:in");

                _controlSystemActor.Connect("keyboardControlBoardButtons:out", "controllerUi:in.buttons");
                _controlSystemActor.Connect("keyboardControlBoardControllers:out", "controllerUi:in.controllers");
            }
        }
    }
}
