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

using Epicycle.Commons.FileSystem;
using Epicycle.Commons.Time;
using RagingBool.Carcosa.Devices;
using System;
using System.Windows;

namespace RagingBool.Carcosa.App
{
    using RagingBool.Carcosa.Core;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IClock _clock;
        private StandardFileSystem _fileSystem;
        private Carcosa _carcosa;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _clock = new HighPrecisionSystemClock();
            _fileSystem = StandardFileSystem.Instance;

            CreateCarcosa();
        }

        private void CreateCarcosa()
        {
            var args = Environment.GetCommandLineArgs();
            var workspacePath = new FileSystemPath(args[1]);

            _carcosa = new Carcosa(_clock, _fileSystem, workspacePath);
        }

        public IClock Clock { get { return _clock; } }

        public ICarcosa Carcosa { get { return _carcosa; } }
    }
}
