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
            return this.MemberwiseClone();
        }

        public ApplicationData DeepCopy()
        {
            ApplicationData other = (ApplicationData)this.MemberwiseClone();
            other.Files = new ObservableCollection<string>();

            foreach (string file in this.Files)
            {
                other.Files.Add(file);
            }

            other.OperationsConfigured = new ObservableCollection<Operation>();

            foreach (var op in this.OperationsConfigured)
            {
                other.OperationsConfigured.Add(op);
            }
            return other;
        }
    }
}
