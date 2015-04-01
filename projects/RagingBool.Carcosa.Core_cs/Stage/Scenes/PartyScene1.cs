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
using Epicycle.Commons.Time;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class PartyScene1 : SceneBase
    {
        private readonly LightSetup _lightSetup;
        private RythmDetector _rythmDetector;
        private RythmGenerator _rythmGenerator;
        private int _tickIndex;

        public PartyScene1(IClock clock, LightSetup lightSetup)
        {
            _lightSetup = lightSetup;

            _rythmDetector = new RythmDetector(clock);
            _rythmDetector.OnNewBpm += OnNewBpm;

            _rythmGenerator = new RythmGenerator();
            _rythmGenerator.ResetAndSetBpm(120);
            _rythmGenerator.OnTick += OnTick;

            _tickIndex = 0;
        }

        void OnNewBpm(double newBpm)
        {
            _rythmGenerator.ResetAndSetBpm(newBpm);
        }

        void OnTick(int tickIndex)
        {
            _tickIndex = tickIndex;
        }

        public override void Enter()
        {
            _rythmGenerator.Reset();
        }

        public override void Exit()
        {

        }

        public override void Update(double dt)
        {
            _rythmGenerator.Update(dt);

            _lightSetup.MonoStrips[0].Intensity = (_tickIndex % 4) == 0 ? 1 : 0;
        }

        public override void HandleSubsceneChange(int newSubscene)
        {
        }

        public override void HandleLightDrumEvent(LightDrumEventArgs eventArgs)
        {
            switch(eventArgs.DrumId)
            {
                case 4:
                    _rythmDetector.Reset();
                    _rythmGenerator.Reset();
                    break;
                case 5:
                    _rythmDetector.Tap();
                    break;
            }
        }
    }
}
