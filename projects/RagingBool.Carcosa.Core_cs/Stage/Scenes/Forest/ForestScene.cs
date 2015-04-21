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

        private readonly ForestEnvironment _environment;

        private readonly IList<ForestCritter> _critters;

        public ForestScene(LightSetup lightSetup)
        {
            _lightSetup = lightSetup;

            _environment = new ForestEnvironment();
            _critters = new List<ForestCritter>();

            InitCritters();
        }

        private void InitCritters()
        {
            var strip = _lightSetup.FadecandyStripAll;

            var critter = CreateCritter(strip[0], null);
            critter.Excitment = 0.2;
            critter.PrimaryHue = 0.66;
            critter.SecondaryHue = 0.86;

            critter = CreateCritter(strip[1], null);
            critter.Excitment = 0.3;
            critter.PrimaryHue = 0.33;
            critter.SecondaryHue = 0.13;

            critter = CreateCritter(strip[2], null);
            critter.Excitment = 0.8;
            critter.PrimaryHue = 0.87;
            critter.SecondaryHue = 0.42;

            critter = CreateCritter(strip[3], null);
            critter.Excitment = 0.1;
            critter.PrimaryHue = 0.21;
            critter.SecondaryHue = 0.56;

            critter = CreateCritter(strip[4], null);
            critter.Excitment = 0.43;
            critter.PrimaryHue = 0.42;
            critter.SecondaryHue = 0.22;

            critter = CreateCritter(strip[5], new IRgbLight[] { strip[6], strip[7] });
            critter.Excitment = 0.55;
            critter.PrimaryHue = 0.1;
            critter.SecondaryHue = 0.4;

            var num = _lightSetup.DmxRgbStrips.Count;
            var step = 1.0 / num;
            var i = 0;

            foreach(var x in _lightSetup.DmxRgbStrips)
            {
                critter = CreateCritter(x, null);
                critter.Excitment = i * step;
                critter.PrimaryHue = i * step;
                critter.SecondaryHue = 0.4;
                i++;
            }
        }

        private ForestCritter CreateCritter(IRgbLight body, IEnumerable<IRgbLight> eyes)
        {
            var critter = new ForestCritter(_environment, body, eyes);
            _critters.Add(critter);

            return critter;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
            UpdateEnvironment(dt);

            foreach(var critter in _critters)
            {
                critter.Update(dt);
            }
        }

        private void UpdateEnvironment(double dt)
        {
            _environment.BaseMagic = GetControl(7);
            _environment.BaseExcitement = GetControl(3);
            _environment.Update(dt);
        }

        public override void HandleSubsceneChange(int newSubscene)
        {
        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
        }
    }
}
