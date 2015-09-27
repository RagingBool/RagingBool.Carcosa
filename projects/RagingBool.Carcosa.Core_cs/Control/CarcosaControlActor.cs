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
using System;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    internal sealed class CarcosaControlActor : UntypedActor
    {
        private readonly ControlSystemActorRef _controlSystemActor;
        private readonly IList<string> _startStopComponents;
        private readonly IList<string> _updateableComponents;

        public CarcosaControlActor()
        {
            _controlSystemActor = new ControlSystemActorRef(Context.ActorOf<ControlSystemActor>("system"));
            _startStopComponents = new List<string>();
            _updateableComponents = new List<string>();

            RegisterComponent(
                "controllerUi",
                typeof(ControllerUiActor),
                Unit.Instance,
                supportsStartStop: true,
                supportsUpdate: true);
        }

        private void RegisterComponent(
            string name, 
            Type componentType, 
            object configuration, 
            bool supportsStartStop = false, 
            bool supportsUpdate = false)
        {
            _controlSystemActor.CreateComponent(name, componentType, configuration);

            if (supportsStartStop)
            {
                _startStopComponents.Add(name);
            }
            if (supportsStartStop)
            {
                _updateableComponents.Add(name);
            }
        }

        protected override void OnReceive(object message)
        {
            if (message is RegisterWindowsKeyboardMessage)
            {
                OnRegisterWindowsKeyboardMessage((RegisterWindowsKeyboardMessage)message);
            }
            else if (message is StartMessage)
            {
                SendToAll(_startStopComponents, message);
            }
            else if (message is StopMessage)
            {
                SendToAll(_startStopComponents, message);
            }
            else if (message is UpdateMessage)
            {
                SendToAll(_updateableComponents, message);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void SendToAll(IList<string> components, object message)
        {
            foreach (var component in components)
            {
                _controlSystemActor.SendMessage(component, message);
            }
        }
        
        private void OnRegisterWindowsKeyboardMessage(RegisterWindowsKeyboardMessage message)
        {
            RegisterComponent("keyboard", typeof(ExternalKeyboardActor<WindowsKey, TimedKey>), message.Keyboard);

            if(message.KeyboardControlBoardConfig != null)
            {
                RegisterComponent(
                    "keyboardControlBoardButtons",
                    typeof(KeyboardControlBoardButtonsActor<WindowsKey>),
                    message.KeyboardControlBoardConfig.ButtonsConfig);

                RegisterComponent(
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
