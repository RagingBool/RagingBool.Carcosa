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

using Epicycle.Input;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Devices.InputControl
{
    public sealed class OverlappingIndicatorBoard<TIndicatorId, TIndicatorValue> : IndicatorBoardBase<TIndicatorId, TIndicatorValue>
    {
        private object _lock = new object();

        private readonly IList<IIndicatorBoard<TIndicatorId, TIndicatorValue>> _indicators;

        public OverlappingIndicatorBoard(TIndicatorValue defaultValue)
            : base(defaultValue, alwaysPropagate: true)
        {
            _indicators = new List<IIndicatorBoard<TIndicatorId, TIndicatorValue>>();
        }

        public void Register(IIndicatorBoard<TIndicatorId, TIndicatorValue> indicator)
        {
            lock (_lock)
            {
                _indicators.Add(indicator);
            }
        }

        protected override void IndicatorValueChanges(TIndicatorId indicatorId, TIndicatorValue value)
        {
            lock (_lock)
            {
                foreach (var indicator in _indicators)
                {
                    indicator.SetIndicatorValue(indicatorId, value);
                }
            }
        }
    }
}
