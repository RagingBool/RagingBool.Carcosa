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
using System.Collections.Generic;

namespace RagingBool.Carcosa.Commons.Control.Akka
{
    public abstract class ControlActor<TConfiguration> : UntypedActor
    {
        private readonly ControlActorRef _controlRef;
        private readonly IActorRef _savedSelf;

        public ControlActor()
        {
            var inputsConfiguration = CreateInputsConfiguration();
            var outputsConfiguration = CreateOutputsConfiguration();
            _controlRef = new ControlActorRef(Self, inputsConfiguration, outputsConfiguration);
            _savedSelf = Self;
        }

        protected override void OnReceive(object message)
        {
            if(message is ConfigureControlMessage)
            {
                OnConfigureControl((ConfigureControlMessage)message);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void OnConfigureControl(ConfigureControlMessage message)
        {
            Configure((TConfiguration) message.Configuration);
        }

        protected abstract void Configure(TConfiguration configuration);

        protected void TellSelfFromOutside(object message)
        {
            _savedSelf.Tell(message, null);
        }

        protected abstract IEnumerable<ControlPortConfiguration> CreateInputsConfiguration();
        protected abstract IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration();
    }
}
