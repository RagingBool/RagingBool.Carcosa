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

using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.Midi;

namespace RagingBool.Carcosa.Core
{
    internal class PartyStage1 : IStage
    {
        private readonly ILpd8 _controller;
        private int _lastController;
        
        public PartyStage1()
        {
            _controller = new MidiLpd8(0, 1);

            _controller.OnButtonEvent += _controller_OnButtonEvent;
            _controller.OnControllerChange += _controller_OnControllerChange;

            _lastController = 255;
        }

        public void Start()
        {
            _controller.Connect();
        }

        public void Update()
        {
            // Nothing to update for now...
        }

        public void Stop()
        {
            _controller.Disconnect();
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
        }
    }
}
