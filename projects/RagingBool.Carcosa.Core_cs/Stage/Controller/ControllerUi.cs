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

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal sealed class ControllerUi
    {
        private readonly ILpd8 _controller;

        private LiveControllerMode _liveMode;

        private IControllerMode _mode;

        public ControllerUi(ILpd8 controller)
        {
            _controller = controller;

            _controller.OnButtonEvent += OnButtonEventHandler;
            _controller.OnControllerChange += OnControllerChangeHandler;

            _liveMode = new LiveControllerMode(_controller);

            _mode = null;
        }

        public void Start()
        {
            SwitchMode(_liveMode);
        }

        public void Stop()
        {
            SwitchMode(_liveMode);
        }

        public void Update()
        {
            if(_mode != null)
            {
                _mode.Update();
            }
        }

        private void SwitchMode(IControllerMode newMode)
        {
            if(_mode != null)
            {
                _mode.Exit();
            }

            _mode = newMode;

            if(_mode != null)
            {
                _mode.Enter();
            }
        }

        private void OnButtonEventHandler(object sender, ButtonEventArgs e)
        {
            if(_mode != null)
            {
                _mode.ProcessButtonEventHandler(e);
            }
        }

        private void OnControllerChangeHandler(object sender, ControllerChangeEventArgs e)
        {
            if (_mode != null)
            {
                _mode.ProcessControllerChangeEvent(e);
            }
        }
    }
}
