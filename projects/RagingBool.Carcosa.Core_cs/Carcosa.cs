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

using Akka.Actor;
using Epicycle.Commons;
using Epicycle.Commons.FileSystem;
using Epicycle.Commons.Time;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Core.Control;
using RagingBool.Carcosa.Core.Stage;
using RagingBool.Carcosa.Core.Workspace;
using RagingBool.Carcosa.Devices;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System.Collections.Generic;
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

        private readonly OverlappingControlBoards _controlBoards;

        private IList<IDevice> _devices;
        private IList<IUpdatable> _updatables;

        private readonly ActorSystem _actorSystem;
        private IActorRef _carcosaActor;

        private bool _isRunning;

        public Carcosa(IClock clock, IFileSystem fileSystem, FileSystemPath workspacePath)
        {
            ArgAssert.NotNull(clock, "clock");
            ArgAssert.NotNull(fileSystem, "fileSystem");
            ArgAssert.NotNull(workspacePath, "workspacePath");

            _clock = clock;
            _fileSystem = fileSystem;
            _workspace = new CarcosaWorkspace(_fileSystem, workspacePath);

            _devices = new List<IDevice>();
            _updatables = new List<IUpdatable>();

            _controlBoards = new OverlappingControlBoards();
            _stage = new PartyStage(_clock, _workspace, _controlBoards);

            _updateThread = new Thread(UpdateThreadLoop);

            _actorSystem = ActorSystem.Create("Carcosa");

            _isRunning = false;
        }

        public ICarcosaWorkspace Workspace
        {
            get { return _workspace; }
        }

        public void Start()
        {
            foreach(var device in _devices)
            {
                device.Connect();
            }

            _stage.Start();
            _carcosaActor = _actorSystem.ActorOf<CarcosaActor>(_workspace.WorkspaceName);
            _carcosaActor.Tell(new StartMessage(_clock.Time));
            _isRunning = true;

            _updateThread.Start();
        }

        public void Stop()
        {
            foreach (var device in _devices)
            {
                device.Disconnect();
            } 
            
            _isRunning = false;
            _carcosaActor.Tell(new StopMessage(_clock.Time));
            _stage.Stop();
            _actorSystem.Shutdown();
        }

        public void AwaitTermination()
        {
            _actorSystem.AwaitTermination();
        }

        public void RegisterDevice(IDevice device)
        {
            _devices.Add(device);
        }

        public void RegisterUpdatable(IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        public void RegisterControlBoard(IControlBoard controlBoard)
        {
            _controlBoards.Register(controlBoard);
        }

        public void RegisterWindowsKeyboard(
            IKeyboard<WindowsKey, TimedKey> keyboard,
            KeyboardControlBoardConfig<WindowsKey> keyboardControlBoardConfig)
        {
            _carcosaActor.Tell(new RegisterWindowsKeyboardMessage(keyboard, keyboardControlBoardConfig));
        }

        private void UpdateThreadLoop()
        {
            while(_isRunning)
            {
                foreach (var updatable in _updatables)
                {
                    updatable.Update();
                }

                _stage.Update();
                Thread.Sleep(10);
            }
        }
    }
}
