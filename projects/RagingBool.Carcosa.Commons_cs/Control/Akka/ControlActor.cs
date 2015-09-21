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

        private readonly IDictionary<string, InputHandler> _inputHandlers;
        private readonly IDictionary<string, OutputManager> _outputs;

        protected delegate void InputHandler(string input, object data);

        public ControlActor()
        {
            var inputsConfiguration = CreateInputsConfiguration();
            var outputsConfiguration = CreateOutputsConfiguration();
            _controlRef = new ControlActorRef(Self, inputsConfiguration, outputsConfiguration);
            _savedSelf = Self;

            _inputHandlers = new Dictionary<string, InputHandler>();

            _outputs = new Dictionary<string, OutputManager>();
            foreach(var config in outputsConfiguration)
            {
                _outputs[config.Name] = new OutputManager();
            }
        }

        protected void RegisterInputHandler(string inputName, InputHandler handler)
        {
            _inputHandlers[inputName] = handler;
        }

        protected void Output(string outputName, object data)
        {
            _outputs[outputName].Output(Context, data);
        }

        protected override void OnReceive(object message)
        {
            if (message is DataMessage)
            {
                OnDataMessage((DataMessage)message);
            }
            else if (message is ConfigureControlMessage)
            {
                OnConfigureControl((ConfigureControlMessage)message);
            }
            else if(message is ConnectToMessage)
            {
                OnConnectToMessage((ConnectToMessage)message);
            }
            else
            {
                Unhandled(message);
            }
        }

        private void OnDataMessage(DataMessage message)
        {
            var inputName = message.InputName;

            if(_inputHandlers.ContainsKey(inputName))
            {
                _inputHandlers[inputName](inputName, message.Data);
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

            var destPath = "../" + destControl;
            _outputs[message.LocalOutput].AddConnection(destPath, destPort);
        }

        protected abstract void Configure(TConfiguration configuration);

        protected void TellSelfFromOutside(object message)
        {
            _savedSelf.Tell(message, null);
        }

        protected abstract IEnumerable<ControlPortConfiguration> CreateInputsConfiguration();
        protected abstract IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration();

        private class OutputManager
        {
            private readonly IList<Connection> _connections;

            public OutputManager()
            {
                _connections = new List<Connection>();
            }

            public void AddConnection(string destPath, string destInputName)
            {
                _connections.Add(new Connection(destPath, destInputName));
            }

            public void Output(IUntypedActorContext context, object data)
            {
                foreach(var connection in _connections)
                {
                    var message = new DataMessage(connection.DestInputName, data);
                    context.ActorSelection(connection.DestPath).Tell(message);
                }
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
    }
}
