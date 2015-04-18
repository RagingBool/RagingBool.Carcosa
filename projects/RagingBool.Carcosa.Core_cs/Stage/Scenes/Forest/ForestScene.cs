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
using RagingBool.Carcosa.Core.Stage.Lights;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes.Forest
{
    internal class ForestScene : SceneBase
    {
        private readonly LightSetup _lightSetup;

        private IList<ForestCritter> _critters;

        public ForestScene(LightSetup lightSetup)
        {
            _lightSetup = lightSetup;

            _critters = new List<ForestCritter>();

            InitCritters();
        }

        private void InitCritters()
        {
            var strip = _lightSetup.FadecandyStripAll;

            var critter = new ForestCritter(strip[0], null);
            critter.PrimaryHue = 0.66;
            _critters.Add(critter);

            critter = new ForestCritter(strip[1], null);
            critter.PrimaryHue = 0.33;
            _critters.Add(critter);

            critter = new ForestCritter(strip[2], null);
            critter.PrimaryHue = 0.87;
            _critters.Add(critter);

            critter = new ForestCritter(strip[3], null);
            critter.PrimaryHue = 0.21;
            _critters.Add(critter);

            critter = new ForestCritter(strip[4], null);
            critter.PrimaryHue = 0.42;
            _critters.Add(critter);

            critter = new ForestCritter(strip[5], new IRgbLight[] { strip[6], strip[7] });
            critter.PrimaryHue = 0.1;
            _critters.Add(critter);
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
            UpdateEnvironment();

            foreach(var critter in _critters)
            {
                critter.Update();
            }
        }

        private void UpdateEnvironment()
        {
            var saturation = GetControl(3);
            var intensity = GetControl(7);
        }

        public override void HandleSubsceneChange(int newSubscene)
        {
        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
        }
    }
}
