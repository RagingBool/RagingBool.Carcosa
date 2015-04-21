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
    internal sealed class ForestEnvironment
    {
        private readonly Oscillator _magicOsc;
        private readonly Oscillator _excitementOsc;

        public ForestEnvironment()
        {
            BaseMagic = 0;
            BaseExcitement = 0;

            Magic = 0;
            Excitement = 0;

            _magicOsc = new Oscillator();
            _magicOsc.Function = Oscillator.FunctionType.Sin;
            _magicOsc.Frequency = 0.05;

            _excitementOsc = new Oscillator();
            _excitementOsc.Function = Oscillator.FunctionType.Sin;
            _excitementOsc.Frequency = 0.1;
        }

        public double BaseMagic { get; set; }
        public double BaseExcitement { get; set; }

        public double Magic { get; private set; }
        public double Excitement { get; private set; }

        public void Update(double dt)
        {
            _magicOsc.Update(dt);
            _excitementOsc.Update(dt);

            Magic = Math.Min(0 + _magicOsc.Value, 1);
            Excitement = Math.Min(0 + _excitementOsc.Value, 1);

            //Magic = BaseMagic;
            //Excitement = BaseExcitement;
        }
    }
}
