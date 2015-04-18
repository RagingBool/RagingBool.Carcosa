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

using Epicycle.Commons.Collections;
using RagingBool.Carcosa.Core.Stage.Lights;
using RagingBool.Carcosa.Core.Stage.Scenes.Signal;
using System.Collections.Generic;
using System.Linq;

namespace RagingBool.Carcosa.Core.Stage.Scenes.Forest
{
    internal class ForestCritter
    {
        private readonly ForestEnvironment _environment;
        private readonly IRgbLight _body;
        private readonly IReadOnlyList<IRgbLight> _eyes;

        private readonly Heartbeat _heartbeat;
        private readonly Oscillator _breathingOsc;
        private readonly Oscillator _hueOsc1;
        private readonly Oscillator _hueOsc2;

        public ForestCritter(ForestEnvironment environment, IRgbLight body, IEnumerable<IRgbLight> eyes)
        {
            _environment = environment;
            _body = body;
            _eyes = eyes != null ? eyes.ToList() : EmptyList<IRgbLight>.Instance;

            Vitality = 0.5;
            PrimaryHue = 0.75;
            SecondaryHue = 0.5;
            EyesHue = 0;

            Magic = 0.1;
            Excitment = 0.1;
            Fear = 0.1;
            Love = 0.1;

            _heartbeat = new Heartbeat();
            _heartbeat.Frequency = 0.7;

            _breathingOsc = new Oscillator();
            _breathingOsc.Function = Oscillator.FunctionType.Sin;
            _breathingOsc.Frequency = 0.4;

            _hueOsc1 = new Oscillator();
            _hueOsc1.Function = Oscillator.FunctionType.Sin;
            _hueOsc1.Frequency = 0.05;

            _hueOsc2 = new Oscillator();
            _hueOsc2.Function = Oscillator.FunctionType.Sin;
            _hueOsc2.Frequency = 0.31;
        }

        public double Vitality { get; set; }
        public double PrimaryHue { get; set; }
        public double SecondaryHue { get; set; }
        public double EyesHue { get; set; }

        public double Magic { get; set; }
        public double Excitment { get; set; }
        public double Fear { get; set; }
        public double Love { get; set; }

        public void Update(double dt)
        {
            _heartbeat.Update(dt);
            _breathingOsc.Update(dt);
            _hueOsc1.Update(dt);
            _hueOsc2.Update(dt);

            var heartbeat = _heartbeat.Value;

            var baseHue = PrimaryHue;
            var hue = baseHue + (_hueOsc1.Value - 0.5) * 0.1 + (_hueOsc2.Value - 0.5) * 0.04;

            var baseIntensity = 0.6 + _environment.Magic / 5;
            var intensity = baseIntensity + _breathingOsc.Value * 0.1 + heartbeat * 0.02;

            var baseSaturation = _environment.Magic;
            var saturation = baseSaturation + heartbeat * 0.05;

            LightUtils.SetRgbLightToHsi(_body, hue, saturation, intensity);
        }
    }
}
