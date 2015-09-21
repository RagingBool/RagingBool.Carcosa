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
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using RagingBool.Carcosa.Devices.Midi;
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
        private KeyboardControlBoard<WindowsKey> _keyboardControlBoard;

        public MainWindow()
        {
            _carcosa = ((App)Application.Current).Carcosa;
            _clock = ((App)Application.Current).Clock;
            _keyboardManager = new WpfKeyboardManager();

            InitKeyboardControlBoard();

            _carcosa.RegisterControlBoard(_keyboardControlBoard);

            //RegisterMidiLpd8();

            InitializeComponent();

            KeyDown += OnKeyEvent;
            KeyUp += OnKeyEvent;
        }

        private void RegisterMidiLpd8()
        {
            var lpd8 = new MidiLpd8(_clock, _carcosa.Workspace.ControllerMidiInPort, _carcosa.Workspace.ControllerMidiOutPort);

            _carcosa.RegisterControlBoard(lpd8);
            _carcosa.RegisterDevice(lpd8);
            _carcosa.RegisterUpdatable(lpd8);
        }

        private void InitKeyboardControlBoard()
        {
            var keyboardConfig = new KeyboardControlBoardButtonsConfig<WindowsKey>(
                buttonKeys: new WindowsKey[] { WindowsKey.Z, WindowsKey.X, WindowsKey.C, WindowsKey.V, WindowsKey.A, WindowsKey.S, WindowsKey.D, WindowsKey.F },
                defaultVelocity: 90, 
                highVelocity: 120,
                highVelocityKey: WindowsKey.LeftShift);

            var controllerValueChangeKeysConfig = new TwoSpeedBidirectionalMovementKeysConfiguration<WindowsKey>(
                slowPositiveDirectionKeyId: WindowsKey.Right, slowNegativeDirectionKeyId: WindowsKey.Left,
                fastPositiveDirectionKeyId: WindowsKey.Up, fastNegativeDirectionKeyId: WindowsKey.Down);

            var controllerConfig = new KeyboardControlBoardControllersConfig<WindowsKey>(
                controllerKeys: new WindowsKey[] { WindowsKey.M, WindowsKey.OemComma, WindowsKey.OemPeriod, WindowsKey.OemQuestion, WindowsKey.K, WindowsKey.L, WindowsKey.Oem1, WindowsKey.OemQuotes },
                valueKeys: new WindowsKey[] { WindowsKey.Oem3, WindowsKey.D1, WindowsKey.D2, WindowsKey.D3, WindowsKey.D4, WindowsKey.D5, WindowsKey.D6, WindowsKey.D7, WindowsKey.D8, WindowsKey.D9, WindowsKey.D0 },
                valueChangeKeysConfig: controllerValueChangeKeysConfig,
                smallValueStep: 1.0 / 1000, bigValueStep: 1.0 / 100);

            var config = new KeyboardControlBoardConfig<WindowsKey>(keyboardConfig, controllerConfig);

            _keyboardControlBoard = new KeyboardControlBoard<WindowsKey>(_keyboardManager, config);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            var workspaceName = _carcosa.Workspace.WorkspaceName;

            Title = String.Format("CARCOSA [{0}]", workspaceName);
            _infoLabel.Content = String.Format("WORKSPACE: {0}", workspaceName);

            _carcosa.Start();

            _carcosa.RegisterWindowsKeyboard(_keyboardManager);
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
