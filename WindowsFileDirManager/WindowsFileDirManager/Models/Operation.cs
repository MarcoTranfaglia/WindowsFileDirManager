namespace WindowsFileDirManager.Models
{
    public class Operation : NotifyBase
    {
        private ActionType _actionType;
        private FilterType _filterType;
        private string _filter;

        public ActionType ActionType 
        {
            get
            {
                return _actionType;
            }
            set
            {
                _actionType = value;
                OnPropertyChanged();
            }
        }

        public FilterType FilterType
        {
            get
            {
                return _filterType;
            }
            set
            {
                _filterType = value;
                OnPropertyChanged();
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

    }
}
