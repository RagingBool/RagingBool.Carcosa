﻿// [[[[INFO>
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
    public sealed class ControlSystemActor : UntypedActor
    {
        private readonly IActorRef _controlNetworkActor;

        public ControlSystemActor()
        {
            _controlNetworkActor = Context.ActorOf<ControlNetworkActor>("network");
        }

        protected override void OnReceive(object message)
        {
            if(message is IControlNetworkMessage)
            {
                _controlNetworkActor.Forward(message);
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}
