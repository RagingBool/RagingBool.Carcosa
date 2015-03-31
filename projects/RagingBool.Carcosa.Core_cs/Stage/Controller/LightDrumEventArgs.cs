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

using System;

namespace RagingBool.Carcosa.Core.Stage.Controller
{
    internal class LightDrumEventArgs : EventArgs
    {
        private readonly int _drumId;
        private readonly ButtonTriggerType _triggerType;
        private readonly int _velocity;

        public LightDrumEventArgs(int drumId, ButtonTriggerType triggerType, int velocity)
        {
            _drumId = drumId;
            _triggerType = triggerType;
            _velocity = velocity;
        }

        public int DrumId
        {
            get { return _drumId; }
        }

        public ButtonTriggerType TriggerType
        {
            get { return _triggerType; }
        }

        public int Velocity
        {
            get { return _velocity; }
        }
    }
}
