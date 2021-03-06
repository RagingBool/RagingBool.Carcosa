﻿// [[[[INFO>
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
using Epicycle.Commons.FileSystemBasedObjects;

namespace RagingBool.Carcosa.Core.Workspace
{
    public class CarcosaWorkspace : DirectoryBasedObjectWithConfiguration<CarcosaWorkspaceConfiguration>, ICarcosaWorkspace
    {
        public CarcosaWorkspace(IFileSystem fileSystem, FileSystemPath path)
            : base(fileSystem, path, configurationFileName: "carcosa_config.yaml", autoInit: false)
        {
        }

        public string WorkspaceName
        {
            get { return Configuration.Name; }
        }

        public int ControllerMidiInPort
        {
            get { return Configuration.ControllerMidiInPort; }
        }

        public int ControllerMidiOutPort
        {
            get { return Configuration.ControllerMidiOutPort; }
        }

        public string SnarkSerialPortName
        {
            get { return Configuration.SnarkSerialPortName; }
        }
    }
}
