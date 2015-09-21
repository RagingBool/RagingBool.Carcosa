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

using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Commons.Control;
using RagingBool.Carcosa.Commons.Control.Akka;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Control
{
    internal sealed class ExternalKeyboardActor<TKeyId, TAdditionalKeyEventData> : ControlActor<IKeyboard<TKeyId, TAdditionalKeyEventData>>
    {
        private IKeyboard<TKeyId, TAdditionalKeyEventData> _externalKeyboard;

        public ExternalKeyboardActor()
        {
            _externalKeyboard = null;
        }

        protected override IEnumerable<ControlPortConfiguration> CreateInputsConfiguration()
        {
            return null;
        }

        protected override IEnumerable<ControlPortConfiguration> CreateOutputsConfiguration()
        {
            return new ControlPortConfiguration[]
            {
                new ControlPortConfiguration("", typeof(KeyEventArgs<TKeyId, TAdditionalKeyEventData>))
            };
        }

        protected override void Configure(IKeyboard<TKeyId, TAdditionalKeyEventData> externalKeyboard)
        {
            if (_externalKeyboard != null)
            {
                _externalKeyboard.OnKeyEvent -= OnExternalKeyEvent;
            }

            _externalKeyboard = externalKeyboard;
            _externalKeyboard.OnKeyEvent += OnExternalKeyEvent;
        }

        protected override void OnReceive(object message)
        {
            if(message is KeyEventArgs<TKeyId, TAdditionalKeyEventData>)
            {
                OnKeyEvent((KeyEventArgs<TKeyId, TAdditionalKeyEventData>)message);
            }
            else
            {
                base.OnReceive(message);
            }
        }

        private void OnKeyEvent(KeyEventArgs<TKeyId, TAdditionalKeyEventData> e)
        {
            base.Output("", e);
        }

        private void OnExternalKeyEvent(object sender, KeyEventArgs<TKeyId, TAdditionalKeyEventData> e)
        {
            TellSelfFromOutside(e);
        }
    }
}
