using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        private readonly IRegistration _registration;
        public RegistrationPageViewModel(INavigationService navigationService, IRegistration registration)
        {
            NavigationService = navigationService;
            _registration = registration;
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

        private bool _isLoginExists = false;
        public bool IsLoginExists
        {
            get { return _isLoginExists; }
            set
            {
                _isLoginExists = value;
                OnPropertyChanged(nameof(IsLoginExists));
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
                        await _registration.SingUpAsync(LoginTextBox, PasswordBox, RoleTextBox);
                        NavigationService.NavigateTo<HomePageViewModel>();
                    }));
            }
        }
    }
}
