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
using RagingBool.Carcosa.Core.Workspace;
using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.Midi;

namespace RagingBool.Carcosa.Core.Stage
{
    internal class PartyStage : IStage
    {
        private readonly IClock _clock;

        private readonly ILpd8 _controller;
        private readonly ISnark _snark;
        private int _lastController;

        public PartyStage(IClock clock, ICarcosaWorkspace workspace)
        {
            _clock = clock;

            _controller = new MidiLpd8(workspace.ControllerMidiInPort, workspace.ControllerMidiOutPort);

            _controller.OnButtonEvent += _controller_OnButtonEvent;
            _controller.OnControllerChange += _controller_OnControllerChange;

            _lastController = 255;

            _snark = new SerialSnark(_clock, workspace.SnarkSerialPortName, 12, 60);
        }

        public void Start()
        {
            _controller.Connect();
            _snark.Connect();
        }

        public void Update()
        {
            _controller.Update();
            _snark.Update();
        }

        public void Stop()
        {
            _controller.Disconnect();
            _snark.Disconnect();
        }

        void _controller_OnButtonEvent(object sender, ButtonEventArgs e)
        {
            if (e.ButtonEventType == ButtonEventType.Pressed)
            {
                _controller.SetKeyLightState(e.ButtonId, _lastController >= 128);
            }
        }

        void _controller_OnControllerChange(object sender, ControllerChangeEventArgs e)
        {
            _lastController = e.Value;

            _snark.SetChannel(e.ControllerId, e.Value);
        }
    }
}
