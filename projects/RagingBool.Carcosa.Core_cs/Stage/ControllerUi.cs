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

namespace RagingBool.Carcosa.Core.Stage
{
    internal sealed class ControllerUi
    {
        private readonly ILpd8 _controller;

        public ControllerUi(ILpd8 controller)
        {
            _controller = controller;

            _controller.OnButtonEvent += OnButtonEventHandler;
            _controller.OnControllerChange += OnControllerChangeHandler;
        }

        public void Start()
        {
            ClearLights();
        }

        public void Stop()
        {
            ClearLights();
        }

        public void Update()
        {

        }

        private void ClearLights()
        {
            for(int i = 0; i < _controller.NumberOfButtons; i++)
            {
                _controller.SetKeyLightState(i, false);
            }
        }

        private void OnButtonEventHandler(object sender, ButtonEventArgs e)
        {
        }

        private void OnControllerChangeHandler(object sender, ControllerChangeEventArgs e)
        {
            _controller.SetKeyLightState(e.ControllerId, e.Value >= 128);
        }
    }
}
