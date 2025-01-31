using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        private const int minPasswordLength = 8;

        private readonly IRegistration _registration;
        private readonly IAccount _account;
        public RegistrationPageViewModel(INavigationService navigationService, IRegistration registration, IAccount account)
        {
            NavigationService = navigationService;
            _registration = registration;
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

        private string _passwordBox;
        public string PasswordBox
        {
            get { return _passwordBox; }
            set
            {
                _passwordBox = value;
                OnPropertyChanged(nameof(PasswordBox));
            }
        }

        private bool _isLoginExists;
        public bool IsLoginExists
        {
            get { return _isLoginExists; }
            set
            {
                _isLoginExists = value;
                OnPropertyChanged(nameof(IsLoginExists));
            }
        }

        private bool _isPasswordLengthEnough;
        public bool IsPasswordLengthEnough
        {
            get { return _isPasswordLengthEnough; }
            set
            {
                _isPasswordLengthEnough = value;
                OnPropertyChanged(nameof(IsPasswordLengthEnough));
            }
        }

        private RelayCommand _signUpCommand;
        public RelayCommand SignUpCommand
        {
            get
            {
                return _signUpCommand ??
                    (_signUpCommand = new RelayCommand(async obj =>
                    {
                        IsLoginExists = await _account.CheckIfLoginExistsAsync(LoginTextBox);
                        if (!IsLoginExists)
                        {
                            IsPasswordLengthEnough = PasswordBox.Length is not >= minPasswordLength;
                            if(!IsPasswordLengthEnough)
                            {
                                await _registration.SingUpAsync(LoginTextBox, PasswordBox, RoleTextBox);
                                LoginTextBox = "";
                                RoleTextBox = "";
                                NavigationService.NavigateTo<HomePageViewModel>();
                            }                            
                        }                        
                    }, x => !LoginTextBox.IsNullOrEmpty() && !PasswordBox.IsNullOrEmpty() ));
            }
        }
    }
}
