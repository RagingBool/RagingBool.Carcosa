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

        private readonly IDictionary<string, Output> _outputs;

        public ControlActor()
        {
            var inputsConfiguration = CreateInputsConfiguration();
            var outputsConfiguration = CreateOutputsConfiguration();
            _controlRef = new ControlActorRef(Self, inputsConfiguration, outputsConfiguration);
            _savedSelf = Self;

            _outputs = new Dictionary<string, Output>();
            foreach(var config in outputsConfiguration)
            {
                _outputs[config.Name] = new Output();
            }
        }

        protected override void OnReceive(object message)
        {
            if (message is GetControlRefMessage)
            {
                Sender.Tell(new ControlRefMessage(_controlRef));
            }
            else if (message is ConfigureControlMessage)
            {
                OnConfigureControl((ConfigureControlMessage)message);
            } else if(message is ConnectToMessage)
            {
                OnConnectToMessage((ConnectToMessage)message);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void OnConfigureControl(ConfigureControlMessage message)
        {
            Configure((TConfiguration)message.Configuration);
        }

        private void OnConnectToMessage(ConnectToMessage message)
        {
            string destControl, destPort;
            ParsingUtils.ParseControlInputId(message.InputId, out destControl, out destPort);

            _outputs[message.LocalOutput].AddConnection(destControl, destPort);
        }

        protected abstract void Configure(TConfiguration configuration);

        protected void TellSelfFromOutside(object message)
        {
            _savedSelf.Tell(message, null);
        }

        protected abstract IEnumerable<ControlPortConfiguration> CreateInputsConfiguration();
        protected abstract IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration();

        private class Output
        {
            private readonly IList<Connection> _connections;

            public Output()
            {
                _connections = new List<Connection>();
            }

            public void AddConnection(string destPath, string destInputName)
            {
                _connections.Add(new Connection(destPath, destInputName));
            }

            private class Connection
            {
                private readonly string _destPath;
                private readonly string _destInputName;

                public Connection(string destPath, string destInputName)
                {
                    _destPath = destPath;
                    _destInputName = destInputName;
                }

                public string DestPath { get { return _destPath; } }
                public string DestInputName { get { return _destInputName; } }
            }
        }

        internal sealed class GetControlRefMessage { }

        internal sealed class ControlRefMessage
        {
            private readonly ControlActorRef _controlRef;

            public ControlRefMessage(ControlActorRef controlRef)
            {
                _controlRef = controlRef;
            }

            public ControlActorRef ControlActorRef { get { return _controlRef; } }
        }
    }
}
