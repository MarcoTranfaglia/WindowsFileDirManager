using WindowsFileDirManager.Properties;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFileDirManager
{
    public class SetupDlgViewModel : NotifyBase
    {
        private readonly Action<bool> _commitDialogAction;

        private string _lastUsedDirectory;
        public SetupDlgViewModel(Action<bool> commitDialogAction)
        {
            _commitDialogAction = commitDialogAction ?? throw new ArgumentNullException(nameof(commitDialogAction));
            CmdSelectFolder = new RelayCommand(x => true, x => ExecuteSelectFolder());

            CmdReset = new RelayCommand(x => true, x => ResetConfig(x));
            CmdCommit = new RelayCommand(x => true, x => Commit(x));

            ReadCurrentConfigFromSettings();
        }

        public string LastUsedDirectory
        {
            get => _lastUsedDirectory;
            set
            {
                _lastUsedDirectory = value;
                OnPropertyChanged();
            }
        }
        public ICommand CmdSelectFolder { get; set; }
        public ICommand CmdReset { get; set; }
        public ICommand CmdCommit { get; set; }

        private void ReadCurrentConfigFromSettings()
        {
            _lastUsedDirectory = Settings.Default.LastUsedDirectory;
        }

        private void ExecuteSelectFolder()
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowser.SelectedPath = LastUsedDirectory;

            var result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                LastUsedDirectory = folderBrowser.SelectedPath;
            }
        }

        private void ResetConfig(object res)
        {
            Settings.Default.Reset();
            _commitDialogAction(true);
        }

        private void Commit(object res)
        {
            Settings.Default.LastUsedDirectory = LastUsedDirectory;
            Settings.Default.Save();
            _commitDialogAction(bool.Parse((string)res));
        }
    }


}