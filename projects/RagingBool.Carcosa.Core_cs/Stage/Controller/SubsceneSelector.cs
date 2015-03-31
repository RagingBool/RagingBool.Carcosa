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
    internal sealed class SubsceneSelector
    {
        private readonly ILpd8 _controller;

        private readonly int _subsceneButtonId;
        private int _phase;

        public SubsceneSelector(ILpd8 controller, int subsceneButtonId)
        {
            _controller = controller;
            _subsceneButtonId = subsceneButtonId;

            SubsceneIndex = 0;
            _phase = 0;
        }

        public int SubsceneIndex { get; set;}

        public void SelectNextSubscene()
        {
            SubsceneIndex = (SubsceneIndex + 1) % 3;
        }

        public void NewFrame()
        {
            Render();

            _phase = (_phase + 1) % 256;
        }

        private void Render()
        {
            bool isOn = false;
            switch(SubsceneIndex)
            {
                case 0:
                    isOn = (_phase % 32) == 0;
                    break;
                case 1:
                    isOn = ((_phase / 2) % 2) == 0;
                    break;
                case 2:
                    isOn = true;
                    break;
            }

            _controller.SetKeyLightState(_subsceneButtonId, isOn);
        }
    }
}
