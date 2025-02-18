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
            WindowName = Properties.Resources.HomeLabel;
        }

        private string _username;
        /// <summary>
        /// A property for binding a user's login and a TextBlock for it.
        /// </summary>
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
        /// <summary>
        /// A property for binding current date and a TextBlock for it.
        /// </summary>
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
        /// <summary>
        /// The command that is called when the user control loads to update the controls.
        /// </summary>
        public RelayCommand LoadUserControlCommand
        {
            get
            {
                return _loadUserControlCommand ??
                    (_loadUserControlCommand = new RelayCommand(obj =>
                    {
                        Username = _account.CustomPrincipal.Identity.Login;
                        TodayDate = DateOnly.FromDateTime(DateTime.Now).ToString();
                    }));
            }
        }
    }
}
