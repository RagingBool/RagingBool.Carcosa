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
using RagingBool.Carcosa.Core.Control;

namespace RagingBool.Carcosa.Core
{
    internal sealed class CarcosaActor : UntypedActor
    {
        private IActorRef _controlActor;

        public CarcosaActor()
        {
            _controlActor = Context.ActorOf<CarcosaControlActor>("control");
        }

        // Running

        protected override void OnReceive(object message)
        {
            if (message is RegisterWindowsKeyboardMessage)
            {
                _controlActor.Forward(message);
            }
            else if (message is StartMessage)
            {
                _controlActor.Forward(message);
            }
            else if (message is StopMessage)
            {
                OnStop((StopMessage)message);
            }
            else
            {
                Unhandled(message);
            }
        }

        // Shutting down
        private void OnReceiveStop(object message)
        {
            Unhandled(message);
        }

        private void OnStop(StopMessage message)
        {
            _controlActor.Forward(message);
            Become(OnReceiveStop);
        }
    }
}
