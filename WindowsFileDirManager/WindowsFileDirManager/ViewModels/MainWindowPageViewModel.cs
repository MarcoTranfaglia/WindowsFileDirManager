using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsFileDirManager.Models;
using WindowsFileDirManager.Properties;
using WindowsFileDirManager.Utility;
using System.Collections.ObjectModel;
using System.Linq;

namespace WindowsFileDirManager
{
    public class MainWindowPageViewModel : NotifyBase
    {
        public bool _isLoading, _changesGridVisible;
        public string _directoryPath, _selectedFilter;
        private FilterType _selectedFilterType;
        private ActionType _selectedActionType;
        public ApplicationData _currentApplicationData;

        public ApplicationData CurrentApplicationData
        {
            get
            {
                return _currentApplicationData;
            }
            set
            {
                _currentApplicationData = value;
                OnPropertyChanged();
            }
        }

        public FilterType SelectedFilterType
        {
            get
            {
                return _selectedFilterType;
            }
            set
            {
                _selectedFilterType = value;
                OnPropertyChanged();
            }
        }

        public ActionType SelectedActionType
        {
            get
            {
                return _selectedActionType;
            }
            set
            {
                _selectedActionType = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
            }
        }

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

        public bool ChangesGridVisible
        {
            get
            {
                return _changesGridVisible;
            }
            set
            {
                _changesGridVisible = value;
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

        public ICommand CmdSetup { get; set; }
        public ICommand CmdPreview { get; set; }
        public ICommand CmdConfirm { get; set; }
        public ICommand CmdAddOperation { get; set; }
        public ICommand CmdDeleteSingleOperation { get; set; }


        //List Action Rename, Delete
        //List Filter startswith         endswith        contains  regexp
        //TestBox 
        //Preview
        //Confirm

        public MainWindowPageViewModel()
        {
            DirectoryPath = Settings.Default.LastUsedDirectory;

            CmdSetup = new RelayCommand(x => !IsLoading, x => Setup());
            CmdPreview = new RelayCommand(x => !IsLoading, x => ExecutePreview());
            CmdConfirm = new RelayCommand(x => !IsLoading, x => ExecuteConfirm());
            CmdAddOperation = new RelayCommand(x => !IsLoading, x => ExecuteAddOperation());
            CmdDeleteSingleOperation = new RelayCommand(x => !IsLoading, x => ExecuteDeleteOperation((Operation)x));


            Task.Run(() => this.LoadDataFromFolder(DirectoryPath))
       .ContinueWith(task =>
       {
           //and set the  property back to false back on the UI thread once the task has finished
           IsLoading = false;
       }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private async Task<bool> LoadDataFromFolder(string folder)
        {
            CurrentApplicationData = FileManagement.ReadFolder(folder);
            CurrentApplicationData.OperationsConfigured = new ObservableCollection<Operation>();


            return true;
        }

        private void ExecutePreview()
        {
            FileManagement.DoChanges(CurrentApplicationData, DirectoryPath, true);
            ChangesGridVisible = true;
        }

        private void ExecuteConfirm()
        {
            FileManagement.DoChanges(CurrentApplicationData, DirectoryPath, false);
        }

        private void ExecuteAddOperation()
        {
            if (string.IsNullOrEmpty(SelectedFilter))
            {
                System.Windows.Forms.MessageBox.Show(Resources.FILTER_MISSING);
                return;
            }

            CurrentApplicationData.OperationsConfigured.Add(new Operation()
            {
                ActionType = SelectedActionType,
                Filter = SelectedFilter,
                FilterType = SelectedFilterType
            });

            //Reset UI Fields
            SelectedFilter = "";
        }

        private void ExecuteDeleteOperation(Operation operationItem)
        {
            CurrentApplicationData.OperationsConfigured.Remove(operationItem);
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

            LoadDataFromFolder(DirectoryPath);
            return true;
        }

    }
}