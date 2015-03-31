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
using Epicycle.Commons.FileSystem;
using Epicycle.Commons.Time;
using RagingBool.Carcosa.Core.Workspace;
using System.Threading;

namespace RagingBool.Carcosa.Core
{
    public sealed class Carcosa : ICarcosa
    {
        private readonly IClock _clock;
        private readonly IFileSystem _fileSystem;
        private readonly CarcosaWorkspace _workspace;

        private readonly IStage _stage;

        private readonly Thread _updateThread;

        private bool _isRunning;

        public Carcosa(IClock clock, IFileSystem fileSystem, FileSystemPath workspacePath)
        {
            ArgAssert.NotNull(clock, "clock");
            ArgAssert.NotNull(fileSystem, "fileSystem");
            ArgAssert.NotNull(workspacePath, "workspacePath");

            _clock = clock;
            _fileSystem = fileSystem;
            _workspace = new CarcosaWorkspace(_fileSystem, workspacePath);

            _stage = new PartyStage1(_clock, _workspace);

            _updateThread = new Thread(UpdateThreadLoop);

            _isRunning = false;
        }

        public ICarcosaWorkspace Workspace
        {
            get { return _workspace; }
        }

        public void Start()
        {
            _stage.Start();
            _isRunning = true;

            _updateThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _stage.Stop();
        }

        private void UpdateThreadLoop()
        {
            while(_isRunning)
            {
                _stage.Update();
                Thread.Sleep(10);
            }
        }
    }
}
