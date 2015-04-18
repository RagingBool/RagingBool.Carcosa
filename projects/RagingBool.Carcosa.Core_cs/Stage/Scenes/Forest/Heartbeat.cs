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

using RagingBool.Carcosa.Core.Stage.Scenes.Signal;
using System;

namespace RagingBool.Carcosa.Core.Stage.Scenes.Forest
{
    internal sealed class Heartbeat : IFunction
    {
        private static readonly double PreBeatLevel = 0.4;
        private static readonly double MainBeatPhase = 0.27;

        private PhaseGenerator _phaseGenerator;
        private EnvelopeAD _preBeatEnv;
        private EnvelopeAD _mainBeatEnv;
        
        private bool _wasMainBeat;
        private double _lastPhase;

        public Heartbeat()
        {
            _phaseGenerator = new PhaseGenerator();
            _preBeatEnv = new EnvelopeAD();
            _mainBeatEnv = new EnvelopeAD();

            _preBeatEnv.AttackTime = 0.05;
            _preBeatEnv.DecayTime = 0.1;

            _mainBeatEnv.AttackTime = 0.05;
            _mainBeatEnv.DecayTime = 0.2;

            _wasMainBeat = false;
            _lastPhase = 0;
        }

        public double Frequency
        {
            get { return _phaseGenerator.Frequency; }
            set { _phaseGenerator.Frequency = value; }
        }

        public double Value { get; private set; }

        public void Update(double dt)
        {
            _phaseGenerator.Update(dt);
            _preBeatEnv.Update(dt);
            _mainBeatEnv.Update(dt);

            var phase = _phaseGenerator.Phase;

            if(phase < _lastPhase)
            {
                _preBeatEnv.Trigger();
                _wasMainBeat = false;
            }

            if (phase > MainBeatPhase && !_wasMainBeat)
            {
                _mainBeatEnv.Trigger();
                _wasMainBeat = true;
            }

            _lastPhase = phase;

            Value = Math.Max(_mainBeatEnv.Value, _preBeatEnv.Value * PreBeatLevel);
        }
    }
}
