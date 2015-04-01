﻿// [[[[INFO>
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
    internal sealed class PartyScene1 : SceneBase
    {
        private readonly LightSetup _lightSetup;

        public PartyScene1(LightSetup lightSetup)
        {
            _lightSetup = lightSetup;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
        }

        public override void HandleSubsceneChange(int newSubscene)
        {
        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
        }
    }
}
