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
using RagingBool.Carcosa.Devices.InputControl;
using System;
using System.Windows.Input;

namespace RagingBool.Carcosa.App
{
    public sealed class WpfKeyboardManager : KeyboardBase<WindowsKey, TimedKey>
    {
        public void ProcessWpfKeyboardEvent(System.Windows.Input.KeyEventArgs eventArgs, double time)
        {
            var key = ConvertKey(eventArgs.Key);
            var eventType = CalcEventType(eventArgs);

            var convertedEventArgs = new KeyEventArgs<WindowsKey, TimedKey>(key, eventType, new TimedKey(time));
            ProcessKeyEvent(convertedEventArgs);
        }

        private static WindowsKey ConvertKey(Key nativeKey)
        {
            return (WindowsKey)nativeKey;
        }

        private static KeyEventType CalcEventType(System.Windows.Input.KeyEventArgs eventArgs)
        {
            if(eventArgs.IsRepeat)
            {
                return KeyEventType.Repeat;
            }
            
            if(eventArgs.IsDown)
            {
                return KeyEventType.Pressed;
            }

            if(eventArgs.IsUp)
            {
                return KeyEventType.Released;
            }

            throw new ArgumentException("Illegal key event!");
        }
    }
}
