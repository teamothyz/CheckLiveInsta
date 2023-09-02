using System.ComponentModel;

namespace InstaChecker
{
    public class CountModel : INotifyPropertyChanged
    {
        public readonly object Locker = new();

        private int _total;
        private int _live;
        private int _die;
        private int _error;
        private int _scanned;

        public int Total
        {
            get => _total;
            set
            {
                _total = value;
                NotifyChanged(nameof(Total));
            }
        }

        public int Live
        {
            get => _live;
            set
            {
                _live = value;
                NotifyChanged(nameof(Live));
            }
        }

        public int Die
        {
            get => _die;
            set
            {
                _die = value;
                NotifyChanged(nameof(Die));
            }
        }

        public int Error
        {
            get => _error;
            set
            {
                _error = value;
                NotifyChanged(nameof(Error));
            }
        }

        public int Scanned
        {
            get => _scanned;
            set
            {
                _scanned = value;
                NotifyChanged(nameof(Scanned));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
