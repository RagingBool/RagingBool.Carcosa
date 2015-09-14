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

using Epicycle.Commons;
using Epicycle.Commons.Time;

namespace RagingBool.Carcosa.Devices
{
    public sealed class MaxFrequencyUpdater : IUpdatable
    {
        private readonly IUpdatable _updatable;
        private readonly IClock _clock;
        private readonly double _maxFrequency;

        private double? _lastUpdateTime;

        public MaxFrequencyUpdater(IUpdatable updatable, IClock clock, double maxFrequency)
        {
            ArgAssert.NotNull(updatable, "updatable");
            ArgAssert.NotNull(clock, "clock");
            ArgAssert.GreaterThan(maxFrequency, "maxFrequency", 0);

            _updatable = updatable;
            _clock = clock;
            _maxFrequency = maxFrequency;

            _lastUpdateTime = null;
        }

        public IUpdatable Updatable
        {
            get { return _updatable; }
        }

        public IClock Clock
        {
            get { return _clock; }
        }

        public double MaxFrequency
        {
            get { return _maxFrequency; }
        }

        public void Update()
        {
            var curTime = _clock.Time;

            if(_lastUpdateTime.HasValue && (curTime - _lastUpdateTime.Value) < (1.0 / _maxFrequency))
            {
                return;
            }

            _updatable.Update();

            _lastUpdateTime = curTime;
        }
    }
}
