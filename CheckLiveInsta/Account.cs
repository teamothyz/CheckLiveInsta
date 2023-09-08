using System.ComponentModel;

namespace CheckLiveInsta
{
    public class Account : INotifyPropertyChanged
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        private int _checkCount;
        public int CheckCount
        {
            get => _checkCount;
            set
            {
                _checkCount = value;
                NotifyChanged(nameof(CheckCount));
            }
        }

        private int _errorCount;
        public int ErrorCount
        {
            get => _errorCount;
            set
            {
                _errorCount = value;
                NotifyChanged(nameof(ErrorCount));
            }
        }

        private LoginStatus _status;
        public LoginStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyChanged(nameof(Status));
            }
        }

        public Dictionary<string, string> Headers { get; set; } = new();

        public string Cookie { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum LoginStatus
    {
        None = 0,
        Success = 1,
        Failed = 2,
        Die = 3,
        HasCookie = 4
    }
}
