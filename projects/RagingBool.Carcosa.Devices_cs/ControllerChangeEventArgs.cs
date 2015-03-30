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

using System;

namespace RagingBool.Carcosa.Devices
{
    public sealed class ControllerChangeEventArgs : EventArgs
    {
        private readonly int _controllerId;
        private readonly int _value;

        public ControllerChangeEventArgs(int controllerId, int value)
        {
            _controllerId = controllerId;
            _value = value;
        }

        public int ControllerId
        {
            get { return _controllerId; }
        }

        public int Value
        {
            get { return _value; }
        }
    }
}