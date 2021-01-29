using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WindowsFileDirManager.Models
{
    public class ApplicationData : NotifyBase, ICloneable
    {
        private ObservableCollection<Operation> _operationsConfigured;

        public ObservableCollection<string> Files { get; set; }

        public ObservableCollection<Operation> OperationsConfigured
        {
            get
            {
                return _operationsConfigured;
            }
            set
            {
                _operationsConfigured = value;
                OnPropertyChanged();
            }
        }

        public ApplicationData() { }
        public ApplicationData(ObservableCollection<string> Files, ObservableCollection<Operation> OperationsConfigured)
        {
            this.Files = Files;
            this.OperationsConfigured = OperationsConfigured;
        }

        public object Clone()
        {
            return new ApplicationData(Files, OperationsConfigured);
        }
    }
}
