using System;
using System.ComponentModel;
using System.Windows;

namespace RagingBool.Carcosa.App
{
    using RagingBool.Carcosa.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICarcosa _carcosa;

        public MainWindow()
        {
            _carcosa = ((App)Application.Current).Carcosa;

            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _carcosa.Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _carcosa.Stop();
        }
    }
}
