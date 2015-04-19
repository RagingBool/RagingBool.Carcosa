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

namespace RagingBool.Carcosa.Core.Stage.Scenes.Signal
{
    public sealed class Oscillator : IFunction
    {
        public enum FunctionType
        {
            SawUp,
            SawDown,
            Square,
            Triangle,
            Sin
        }

        private readonly PhaseGenerator _phaseGenerator;

        public Oscillator()
        {
            _phaseGenerator = new PhaseGenerator();

            Value = 0;
            Function = Oscillator.FunctionType.Sin;
        }

        public double Frequency
        {
            get { return _phaseGenerator.Frequency; }
            set { _phaseGenerator.Frequency = value; }
        }

        public FunctionType Function { get; set; }

        public double Value { get; private set; }

        public void Update(double dt)
        {
            _phaseGenerator.Update(dt);

            Value = ShapeValue(_phaseGenerator.Phase, Function);
        }

        private static double ShapeValue(double phase, FunctionType function)
        {
            switch (function)
            {
                case FunctionType.SawUp:
                    return phase;
                case FunctionType.SawDown:
                    return 1 - phase;
                case FunctionType.Square:
                    return phase < 0.5 ? 0 : 1;
                case FunctionType.Triangle:
                    var doublePhase = phase * 2;
                    return doublePhase <= 1.0 ? doublePhase : 2 - doublePhase;
                case FunctionType.Sin:
                    return Math.Sin(phase * Math.PI * 2) / 2 + 0.5;
                default:
                    return phase;
            }
        }
    }
}
