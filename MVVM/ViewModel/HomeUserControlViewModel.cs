using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Authentication;

namespace ProjectTracker.MVVM.ViewModel
{
    public class HomeUserControlViewModel : ViewModelBase
    {
        private readonly IAccountService _account;
        public HomeUserControlViewModel(IAccountService account)
        {
            _account = account;
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _todayDate;
        public string TodayDate
        {
            get { return _todayDate; }
            set
            {
                _todayDate = value;
                OnPropertyChanged(nameof(TodayDate));
            }
        }

        private RelayCommand _loadUserControlCommand;
        public RelayCommand LoadUserControlCommand
        {
            get
            {
                return _loadUserControlCommand ??
                    (_loadUserControlCommand = new RelayCommand(obj =>
                    {
                        Username = _account.CurrentUser.Login;
                        TodayDate = DateOnly.FromDateTime(DateTime.Now).ToString();
                    }));
            }
        }
    }
}
