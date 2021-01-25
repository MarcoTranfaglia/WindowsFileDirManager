using System.Windows;

namespace WindowsFileDirManager
{
    /// <summary>
    /// Interaction logic for SetupDlg.xaml
    /// </summary>
    public partial class SetupDlg : Window
    {
        public SetupDlg()
        {
            DataContext = new SetupDlgViewModel(CommitDialog);
            InitializeComponent();
        }

        private void CommitDialog(bool res)
        {
            this.DialogResult = res;
        }
    }
}
