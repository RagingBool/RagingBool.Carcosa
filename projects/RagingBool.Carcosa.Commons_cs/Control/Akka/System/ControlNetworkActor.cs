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

namespace RagingBool.Carcosa.Commons.Control.Akka.System
{
    internal class ControlNetworkActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is CreateComponentMesssage)
            {
                OnCreateComponentMesssage((CreateComponentMesssage)message);
            }
            else if (message is ConnectMesssage)
            {
                OnConnectMesssage((ConnectMesssage)message);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void OnCreateComponentMesssage(CreateComponentMesssage message)
        {
            var actor = Context.ActorOf(new Props(message.ComponentType), message.Name);

            actor.Tell(new ConfigureControlMessage(message.Configuration));
        }

        private void OnConnectMesssage(ConnectMesssage message)
        {
            // TODO
        }
    }
}
