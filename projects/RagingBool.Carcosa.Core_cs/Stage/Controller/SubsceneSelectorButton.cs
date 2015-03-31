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
    internal sealed class SubsceneSelectorButton : Button
    {
        public SubsceneSelectorButton(ILpd8 controller, int subsceneButtonId)
            : base(controller, subsceneButtonId, ButtonTriggerBehaviour.OnPush)
        {
            SubsceneId = 0;
        }

        public int SubsceneId { get; set; }

        protected override bool? RenderFrame(int phases)
        {
            switch (SubsceneId)
            {
                case 0:
                    return (phases % 32) == 0;
                case 1:
                    return ((phases / 2) % 2) == 0;
                case 2:
                    return true;
                default:
                    return false;
            }
        }
    }
}
