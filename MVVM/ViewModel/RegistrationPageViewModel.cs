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

        private readonly IRegistrationService _registration;
        private readonly IAccountService _account;
        public RegistrationPageViewModel(INavigationService navigationService, IRegistrationService registration, IAccountService account)
        {
            NavigationService = navigationService;
            _registration = registration;
            _account = account;
        }

        private INavigationService _navigationService;
        /// <summary>
        /// A property for navigating between views.
        /// </summary>
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
        /// <summary>
        /// A property for binding a user's login and a TextBox for it.
        /// </summary>
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
        /// <summary>
        /// A property for binding a user's role and a TextBox for it.
        /// </summary>
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
        /// <summary>
        /// A property for binding a user's new password and a PasswordBox for it.
        /// </summary>
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
        /// <summary>
        /// A property for binding a result of cheking existence of login in database and an IsOpen property of certain Popup.
        /// </summary>
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
        /// <summary>
        /// A property for binding a result of cheking the lenth of new password and an IsOpen property of certain Popup.
        /// </summary>
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
        /// <summary>
        /// The command that is called when sign up button is clicked.
        /// </summary>
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
