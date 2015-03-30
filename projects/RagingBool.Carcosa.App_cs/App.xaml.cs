using Epicycle.Commons.FileSystem;
using Epicycle.Commons.Time;
using System.Windows;

namespace RagingBool.Carcosa.App
{
    using RagingBool.Carcosa.Core;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SystemClock _clock;
        private StandardFileSystem _fileSystem;
        private Carcosa _carcosa;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _clock = new SystemClock();
            _fileSystem = StandardFileSystem.Instance;

            _carcosa = new Carcosa(_clock, _fileSystem);
        }

        public ICarcosa Carcosa
        {
            get { return _carcosa; }
        }
    }
}
