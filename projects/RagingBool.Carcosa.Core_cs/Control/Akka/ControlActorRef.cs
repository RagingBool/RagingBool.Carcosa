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

namespace RagingBool.Carcosa.Core.Control.Akka
{
    internal sealed class ControlActorRef : IControlComponent
    {
        private readonly IActorRef _actorRef;

        private readonly IDictionary<string, ControlActorInput> _inputs;
        private readonly IDictionary<string, ControlActorOutput> _outputs;

        public ControlActorRef(
            IActorRef actorRef,
            IEnumerable<ControlPortConfiguration> inputsConfiguration,
            IEnumerable<ControlPortConfiguration> outputsConfiguration)
        {
            _actorRef = actorRef;

            // Init inputs
            _inputs = new Dictionary<string, ControlActorInput>();
            if (inputsConfiguration != null)
            {
                foreach(var config in inputsConfiguration)
                {
                    _inputs[config.Name] = new ControlActorInput(this, config);
                }
            }

            // Init outputs
            _outputs = new Dictionary<string, ControlActorOutput>();
            if (outputsConfiguration != null)
            {
                foreach (var config in outputsConfiguration)
                {
                    _outputs[config.Name] = new ControlActorOutput(this, config);
                }
            }
        }

        public IActorRef ActorRef { get { return _actorRef; } }

        public void ConnectTo(string outputName, ControlActorInput toInput)
        {
            // TODO
        }

        public IControlInput GetInput(string name)
        {
            return _inputs[name];
        }

        public IControlOutput GetOutput(string name)
        {
            return _outputs[name];
        }
    }
}
