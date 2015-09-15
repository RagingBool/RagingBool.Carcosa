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

using RagingBool.Carcosa.Core.Stage.Controller;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal abstract class SceneBase : IScene
    {
        private const int NumberOfControls = 8;
        private const int ControlDefaultRawValue = 128;

        private double[] _controls;

        public SceneBase()
        {
            _controls = new double[NumberOfControls];

            for(var i = 0; i < NumberOfControls; i++)
            {
                SetControl(i, ControlDefaultRawValue);
            }
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update(double dt);

        private void SetControl(int id, double value)
        {
            _controls[id] = value;
        }

        public double GetControl(int id)
        {
            return _controls[id];
        }

        public abstract void HandleSubsceneChange(int newSubscene);
        public abstract void HandleLightDrumEvent(LightDrumEventArgs eventArgs);

        public void HandleControlParameterValueChange(ControlParameterValueChangeEventArgs eventArgs)
        {
            SetControl(eventArgs.ControlParameterId, eventArgs.NewValue);
        }
    }
}
