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

using RagingBool.Carcosa.Commons.Akka;

namespace RagingBool.Carcosa.Core
{
    internal sealed class CarcosaActor : InitializableActor<CarcosaInitMessage>
    {
        // Init
        protected override void Init(CarcosaInitMessage message)
        {
            // TODO
        }

        // Running
        protected override void OnRecieveInitialized(object message)
        {
            if (message is CarcosaShutDownMessage)
            {
                OnShutDown();
            }
            else
            {
                Unhandled(message);
            }
        }

        // Shutting down
        private void OnRecieveShutDown(object message)
        {
            Unhandled(message);
        }

        private void OnShutDown()
        {
            Become(OnRecieveShutDown);
            ShutDown();
        }

        private void ShutDown()
        {
            // TODO
        }
    }
}
