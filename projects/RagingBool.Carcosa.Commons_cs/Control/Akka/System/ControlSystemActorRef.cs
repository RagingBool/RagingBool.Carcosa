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
using System;

namespace RagingBool.Carcosa.Commons.Control.Akka.System
{
    public class ControlSystemActorRef
    {
        private readonly IActorRef _actorRef;

        public ControlSystemActorRef(IActorRef actorRef)
        {
            _actorRef = actorRef;
        }

        public void CreateComponent(string name, Type componentType, object configuration)
        {
            _actorRef.Tell(new CreateComponentMesssage(name, componentType, configuration));
        }

        public void Connect(string outputId, string inputId)
        {
            _actorRef.Tell(new ConnectMesssage(outputId, inputId));
        }

        public void SendMessage(string name, object message)
        {
            _actorRef.Tell(new SendMessageMessage(name, message));
        }
    }
}
