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

using Epicycle.Commons.Time;
using RagingBool.Carcosa.Core.Stage.Controller;
using RagingBool.Carcosa.Core.Workspace;
using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.Midi;

namespace RagingBool.Carcosa.Core.Stage
{
    internal sealed class PartyStage : IStage
    {
        private readonly IClock _clock;

        private readonly ILpd8 _controller;
        private readonly ISnark _snark;

        private readonly ControllerUi _controllerUi;

        public PartyStage(IClock clock, ICarcosaWorkspace workspace)
        {
            _clock = clock;

            _controller = new MidiLpd8(workspace.ControllerMidiInPort, workspace.ControllerMidiOutPort);
            _snark = new SerialSnark(_clock, workspace.SnarkSerialPortName, 12, 60);

            _controllerUi = new ControllerUi(_controller);
        }

        public void Start()
        {
            _controller.Connect();
            _snark.Connect();

            _controllerUi.Start();
        }

        public void Update()
        {
            _controller.Update();
            _snark.Update();

            _controllerUi.Update();
        }

        public void Stop()
        {
            _controllerUi.Stop();

            _controller.Disconnect();
            _snark.Disconnect();
        }
    }
}
