using System.Windows;
using System.Windows.Input;
using WindowsFileDirManager.Properties;

namespace WindowsFileDirManager
{
    public class MainWindowPageViewModel : NotifyBase
    {
        public bool _isLoading;
        public string _directoryPath;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string DirectoryPath
        {
            get
            {
                return _directoryPath;
            }
            set
            {
                _directoryPath = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetupCommand { get; set; }
        public ICommand Preview { get; set; }
        public ICommand Confirm { get; set; }


        //List Action Rename, Delete
        //List Filter startswith         endswith        contains  regexp
        //TestBox 
        //Preview
        //Confirm


        public MainWindowPageViewModel()
        {
            SetupCommand = new RelayCommand(x => !IsLoading, x => Setup());
            DirectoryPath = Settings.Default.LastUsedDirectory;
        }

        private void Setup(bool forceInitialization = false)
        {
            var dlg = new SetupDlg();
            var dlgRes = dlg.ShowDialog();
            DirectoryPath = Settings.Default.LastUsedDirectory;
            if (dlgRes == true)
            {
                if (InitializeApp())
                {
                    MessageBox.Show("The settings have been updated.");
                }
                else
                {
                    Setup(true);
                }
            }
            else if (forceInitialization)
            {
                MessageBox.Show("You can't continue until all configuration values are set up.");
                Setup(true);
            }
        }

        private bool InitializeApp()
        {
            if (string.IsNullOrEmpty(DirectoryPath))
            {
                MessageBox.Show("Setup all the configuration values before proceeding.");
                return false;
            }

            //LoadDataFromFolder(DirectoryPath, true);
            return true;
        }

    }
}