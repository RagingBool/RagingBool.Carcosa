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

namespace RagingBool.Carcosa.Core.Stage.Scenes.Signal
{
    public sealed class EnvelopeAD : IFunction
    {
        public enum EnvelopeState
        {
            Idle,
            Attack,
            Decay,
        }

        private EnvelopeState _state;
        private double _stateTime;

        public EnvelopeAD()
        {
            AttackTime = 0.1;
            DecayTime = 0.1;

            Value = 0;
            _stateTime = 0;
        }

        public double AttackTime { get; set; }
        public double DecayTime { get; set; }

        public double Value { get; private set; }

        public void Trigger()
        {
            _state = EnvelopeState.Attack;
            _stateTime = 0;
        }

        public void Update(double dt)
        {
            if(_state == EnvelopeState.Idle)
            {
                Value = 0;
                return;
            }

            var totalStateTime = (_state == EnvelopeState.Attack) ? AttackTime : DecayTime;

            var phase = _stateTime / totalStateTime;

            Value = (_state == EnvelopeState.Attack) ? phase : 1 - phase;

            _stateTime += dt;

            if(_stateTime >= totalStateTime)
            {
                _stateTime -= totalStateTime;
                _state = _state == EnvelopeState.Attack ? EnvelopeState.Decay : EnvelopeState.Idle;
            }
        }
    }
}
