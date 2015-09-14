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

using Moq;
using NUnit.Framework;

namespace RagingBool.Carcosa.Devices
{
    [TestFixture]
    public class MultipleUpdatablesTest
    {
        [Test]
        public void Update_updates_all_the_child_updatables()
        {
            var updatableMock1 = new Mock<IUpdatable>();
            var updatableMock2 = new Mock<IUpdatable>();
            var updatableMock3 = new Mock<IUpdatable>();

            var multipleUpdatables = new MultipleUpdatables(new IUpdatable[] { 
                updatableMock1.Object,
                updatableMock2.Object,
                updatableMock3.Object});

            multipleUpdatables.Update();

            updatableMock1.Verify(m => m.Update(), Times.Once);
            updatableMock2.Verify(m => m.Update(), Times.Once);
            updatableMock3.Verify(m => m.Update(), Times.Once);
        }
    }
}
