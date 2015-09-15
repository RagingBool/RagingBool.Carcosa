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

namespace RagingBool.Carcosa.Devices.InputControl
{
    public sealed class TwoSpeedBidirectionalMovementKeysConfiguration<TKeyId>
    {
        private readonly BidirectionalMovementKeysConfiguration<TKeyId> _slowMovementKeysConfiguration;
        private readonly BidirectionalMovementKeysConfiguration<TKeyId> _fastMovementKeysConfiguration;

        public TwoSpeedBidirectionalMovementKeysConfiguration(
            BidirectionalMovementKeysConfiguration<TKeyId> slowMovementKeysConfiguration,
            BidirectionalMovementKeysConfiguration<TKeyId> fastMovementKeysConfiguration)
        {
            _slowMovementKeysConfiguration = slowMovementKeysConfiguration;
            _fastMovementKeysConfiguration = fastMovementKeysConfiguration;
        }

        public TwoSpeedBidirectionalMovementKeysConfiguration(
            TKeyId slowNegativeDirectionKeyId, TKeyId slowPositiveDirectionKeyId,
            TKeyId fastNegativeDirectionKeyId, TKeyId fastPositiveDirectionKeyId)
            : this(
                new BidirectionalMovementKeysConfiguration<TKeyId>(slowNegativeDirectionKeyId, slowPositiveDirectionKeyId),
                new BidirectionalMovementKeysConfiguration<TKeyId>(fastNegativeDirectionKeyId, fastPositiveDirectionKeyId)
            ) { }

        public BidirectionalMovementKeysConfiguration<TKeyId> SlowMovementKeysConfiguration { get { return _slowMovementKeysConfiguration; } }
        public BidirectionalMovementKeysConfiguration<TKeyId> FastMovementKeysConfiguration { get { return _fastMovementKeysConfiguration; } }
    }
}
