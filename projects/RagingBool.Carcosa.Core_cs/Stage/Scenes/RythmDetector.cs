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

using Epicycle.Commons.Time;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class RythmDetector
    {
        public delegate void NewBpmHandler(double newBpm);

        private readonly IClock _clock;

        private readonly List<double> _tapTimes;

        public RythmDetector(IClock clock)
        {
            _clock = clock;

            _tapTimes = new List<double>();
        }

        public event NewBpmHandler OnNewBpm;

        public void Reset()
        {
            _tapTimes.Clear();
        }

        public void Tap()
        {
            _tapTimes.Add(_clock.Time);
            if(_tapTimes.Count >= 4)
            {
                var dt = _tapTimes[_tapTimes.Count - 1] - _tapTimes[_tapTimes.Count - 2];
                var newBpm = 60.0 / dt;

                if(OnNewBpm != null)
                {
                    OnNewBpm(newBpm);
                }
            }
        }
    }
}
