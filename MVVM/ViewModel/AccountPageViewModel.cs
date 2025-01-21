using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {
        private readonly IAccount _account;
        public AccountPageViewModel(INavigationService navigationService, IAccount account)
        {
            NavigationService = navigationService;
            _account = account;
        }

        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                _navigationService = value;
                OnPropertyChanged(nameof(NavigationService));
            }
        }

        private string _loginTextBox;
        public string LoginTextBox
        {
            get { return _loginTextBox; }
            set
            {
                _loginTextBox = value;
                OnPropertyChanged(nameof(LoginTextBox));
            }
        }

        private string _roleTextBox;
        public string RoleTextBox
        {
            get { return _roleTextBox; }
            set
            {
                _roleTextBox = value;
                OnPropertyChanged(nameof(RoleTextBox));
            }
        }

        private string _newPasswordBox;
        public string NewPasswordBox
        {
            get { return _newPasswordBox; }
            set
            {
                _newPasswordBox = value;
                OnPropertyChanged(nameof(NewPasswordBox));
            }
        }

        private string _repeatPasswordBox;
        public string RepeatPasswordBox
        {
            get { return _repeatPasswordBox; }
            set
            {
                _repeatPasswordBox = value;
                OnPropertyChanged(nameof(RepeatPasswordBox));
            }
        }

        private bool _passwordsNotEqual;
        public bool PasswordsNotEqual
        {
            get { return _passwordsNotEqual; }
            set
            {
                _passwordsNotEqual = value;
                OnPropertyChanged(nameof(PasswordsNotEqual));
            }
        }

        private bool _loginExists;
        public bool LoginExists
        {
            get { return _loginExists; }
            set
            {
                _loginExists = value;
                OnPropertyChanged(nameof(LoginExists));
            }
        }

        private RelayCommand _loadAccountPageCommand;
        public RelayCommand LoadAccountPageCommand
        {
            get
            {
                return _loadAccountPageCommand ??
                    (_loadAccountPageCommand = new RelayCommand(obj =>
                    {
                        LoginTextBox = _account.CurrentUser.Login;
                        RoleTextBox = _account.CurrentUser.Role;
                    }));
            }
        }

        private RelayCommand _saveNewPersonalInfoCommand;
        public RelayCommand SaveNewPersonalInfoCommand
        {
            get
            {
                return _saveNewPersonalInfoCommand ??
                    (_saveNewPersonalInfoCommand = new RelayCommand(async obj =>
                    {
                        LoginExists = await _account.CheckIfLoginExistsAsync(LoginTextBox);

                        if (!LoginExists)
                        {
                            await _account.UpdateUserPersonalInfoAsync(LoginTextBox, RoleTextBox);
                        }
                    }));
            }
        }

        private RelayCommand _saveNewPasswordCommand;
        public RelayCommand SaveNewPasswordCommand
        {
            get
            {
                return _saveNewPasswordCommand ??
                    (_saveNewPasswordCommand = new RelayCommand(async obj =>
                    {
                        PasswordsNotEqual = !NewPasswordBox.Equals(RepeatPasswordBox);
                        if (!PasswordsNotEqual)
                        {
                            await _account.UpdateUserPasswordAsync(NewPasswordBox);
                            NewPasswordBox = "";
                            RepeatPasswordBox = "";
                        }
                    }));
            }
        }

        private RelayCommand _logOutCommand;
        public RelayCommand LogOutCommand
        {
            get
            {
                return _logOutCommand ??
                    (_logOutCommand = new RelayCommand(async obj =>
                    {
                        NavigationService.NavigateTo<AutorizationPageViewModel>();
                        _account.CurrentUser = null;
                    }));
            }
        }

        private RelayCommand _deleteAccountCommand;
        public RelayCommand DeleteAccountCommand
        {
            get
            {
                return _deleteAccountCommand ??
                    (_deleteAccountCommand = new RelayCommand(async obj =>
                    {
                        await _account.DeleteAccountAsync();
                        NavigationService.NavigateTo<AutorizationPageViewModel>();
                        _account.CurrentUser = null;
                    }));
            }
        }
    }
}
