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
        private ICarcosa _carcosa;
        private WpfKeyboardManager _keyboardManager;

        public MainWindow()
        {
            _carcosa = ((App)Application.Current).Carcosa;
            _keyboardManager = new WpfKeyboardManager();

            InitializeComponent();
            KeyDown += OnKeyEvent;
            KeyUp += OnKeyEvent;
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
            _keyboardManager.KeyboardEvent(e);
        }
    }
}
