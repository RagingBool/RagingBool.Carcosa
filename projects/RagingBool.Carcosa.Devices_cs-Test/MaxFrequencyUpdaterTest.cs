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
using Moq;
using NUnit.Framework;

namespace RagingBool.Carcosa.Devices
{
    [TestFixture]
    public class MaxFrequencyUpdaterTest
    {
        private Mock<IUpdatable> _updatableMock;
        private ManualClock _clock;
        private double _maxFrequency;

        private MaxFrequencyUpdater _updater;

        [SetUp]
        public void SetUp()
        {
            _updatableMock = new Mock<IUpdatable>(MockBehavior.Strict);
            _clock = new ManualClock();
            _clock.Time = 10000;
            _maxFrequency = 2.0;

            _updater = new MaxFrequencyUpdater(_updatableMock.Object, _clock, _maxFrequency);
        }

        [Test]
        public void Updatable_returns_object_from_ctor()
        {
            Assert.That(_updater.Updatable, Is.SameAs(_updatableMock.Object));
        }

        [Test]
        public void Clock_returns_object_from_ctor()
        {
            Assert.That(_updater.Clock, Is.SameAs(_clock));
        }

        [Test]
        public void MaxFrequency_returns_object_from_ctor()
        {
            Assert.That(_updater.MaxFrequency, Is.EqualTo(_maxFrequency));
        }

        [Test]
        public void Update_always_updates_on_first_time()
        {
            _clock.Advance(0.1);
            AllowUpdate();

            TestUpdate(true);
        }

        [Test]
        public void Update_doesnt_propagate_if_dt_is_too_small()
        {
            AllowUpdate();
            _updater.Update();
            
            _clock.Advance(0.4);
            TestUpdate(false);
        }

        [Test]
        public void Update_propagates_if_dt_is_big_enough()
        {
            AllowUpdate();
            _updater.Update();
            _clock.Advance(0.4);
            _updater.Update();

            _clock.Advance(0.2);
            TestUpdate(true);
        }

        [Test]
        public void Update_doesnt_propagate_if_dt_is_too_small_since_last_update()
        {
            AllowUpdate();
            _updater.Update();
            _clock.Advance(1);
            _updater.Update();

            _clock.Advance(0.4);
            TestUpdate(false);
        }

        [Test]
        public void Update_propagates_if_dt_is_big_enough_since_last_update()
        {
            AllowUpdate();
            _updater.Update();
            _clock.Advance(1);
            _updater.Update();

            _clock.Advance(0.6);
            TestUpdate(true);
        }

        private void TestUpdate(bool expectUpdatePropagation)
        {
            _updatableMock.ResetCalls();
            _updater.Update();
            _updatableMock.Verify(m => m.Update(), expectUpdatePropagation ? Times.Once() : Times.Never());
        }

        private void ExpectNoUpdate()
        {
            _updatableMock.ResetCalls();
            _updater.Update();
            _updatableMock.Verify(m => m.Update(), Times.Never);
        }

        private void AllowUpdate()
        {
            _updatableMock.Setup(m => m.Update()).Verifiable();
        }

        // TODO: Use Epicycle version when available
        private class ManualClock : IClock
        {
            public ManualClock()
            {
                Time = 0;
            }

            public double Time { get; set; }

            public void Advance(double amount)
            {
                Time += amount;
            }
        }
    }
}
