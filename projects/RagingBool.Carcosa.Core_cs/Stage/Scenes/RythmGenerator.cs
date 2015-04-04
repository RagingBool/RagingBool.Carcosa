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

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class RythmGenerator
    {
        public delegate void OnTickHandler(int tickIndex);

        private double _tickCountdown;
        private double _bpm;
        private int _tickIndex;

        public RythmGenerator()
        {
            ResetAndSetBpm(120);
        }

        public event OnTickHandler OnTick;

        public void ResetAndSetBpm(double newBpm)
        {
            _bpm = newBpm;
            Reset();
        }

        public void Reset()
        {
            _tickCountdown = 0;
            _tickIndex = 0;
        }

        public void Update(double dt)
        {
            _tickCountdown -= dt;

            if(_tickCountdown < 0)
            {
                _tickCountdown += (60.0 / _bpm) / 4.0;

                if(OnTick != null)
                {
                    OnTick(_tickIndex);
                }

                _tickIndex = (_tickIndex + 1) % 16;
            }
        }

    }
}
