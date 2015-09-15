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
using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RagingBool.Carcosa.App
{
    using RagingBool.Carcosa.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IClock _clock;
        private readonly ICarcosa _carcosa;
        private readonly WpfKeyboardManager _keyboardManager;
        private KeyboardControlBoard<Key> _keyboardControlBoard;

        public MainWindow()
        {
            _carcosa = ((App)Application.Current).Carcosa;
            _clock = ((App)Application.Current).Clock;
            _keyboardManager = new WpfKeyboardManager();

            InitKeyboardControlBoard();

            InitializeComponent();
            KeyDown += OnKeyEvent;
            KeyUp += OnKeyEvent;
        }

        private void InitKeyboardControlBoard()
        {
            _keyboardControlBoard = new KeyboardControlBoard<Key>(
                _keyboardManager,
                buttonKeys: new Key[] { Key.Z, Key.X, Key.C, Key.V, Key.A, Key.S, Key.D, Key.F},
                defaultVelocity: 90, 
                highVelocity: 120,
                highVelocityKey: Key.LeftShift,
                controllerKeys: new Key[] { Key.M, Key.OemComma, Key.OemPeriod, Key.OemQuestion, Key.K, Key.L, Key.Oem1, Key.OemQuotes },
                faderKeys: new Key[] { Key.Oem3, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0 },
                faderUpKey: Key.Right, faderDownKey: Key.Left, faderFastUpKey: Key.Up, faderFastDownKey: Key.Down,
                faderSmallStep: 1.0 / 1000, faderBigStep: 1.0 / 100);

            _keyboardControlBoard.Buttons.OnKeyEvent += OnButtonEvent;
            _keyboardControlBoard.OnControllerChange += OnControllerChange;
        }

        private void OnButtonEvent(object sender, KeyEventArgs<int, TimedKeyVelocity> e)
        {
            System.Console.WriteLine("Key: {0} - {1} ({2})", e.KeyId, e.EventType, e.AdditionalData.Velocity);
            System.Console.WriteLine("     dt: {0}", _clock.Time - e.AdditionalData.Time);
        }

        private void OnControllerChange(object sender, ControllerChangeEventArgs<int, double> e)
        {
            System.Console.WriteLine("Key: {0} = {1}", e.ControllerId, e.Value);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            var workspaceName = _carcosa.Workspace.WorkspaceName;

            Title = String.Format("CARCOSA [{0}]", workspaceName);
            _infoLabel.Content = String.Format("WORKSPACE: {0}", workspaceName);

            _carcosa.Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _carcosa.Stop();
        }

        private void OnKeyEvent(object sender, KeyEventArgs e)
        {
            var time = _clock.Time;
            _keyboardManager.ProcessWpfKeyboardEvent(e, time);
        }
    }
}
