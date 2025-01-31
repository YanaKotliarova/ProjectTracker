using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AutorizationPageViewModel : ViewModelBase
    {
        private readonly IAutorization _autorization;
        public AutorizationPageViewModel(INavigationService navigationService, IAutorization autorization)
        {
            NavigationService = navigationService;
            _autorization = autorization;
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

        private bool _isLoginUnsuccessful = false;
        public bool IsLoginUnsuccessful
        {
            get { return _isLoginUnsuccessful; }
            set
            {
                _isLoginUnsuccessful = value;
                OnPropertyChanged(nameof(IsLoginUnsuccessful));
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                    (_loginCommand = new RelayCommand(async obj =>
                    {
                        IsLoginUnsuccessful = !await _autorization.LogInAsync(LoginTextBox, PasswordBox);
                        if (!IsLoginUnsuccessful)
                        {
                            NavigationService.NavigateTo<HomePageViewModel>();
                            LoginTextBox = "";
                        }                                             
                    }, x => !LoginTextBox.IsNullOrEmpty() && !PasswordBox.IsNullOrEmpty()));
            }
        }

        private RelayCommand _goToRegistrationPageCommand;
        public RelayCommand GoToRegistrationPageCommand
        {
            get
            {
                return _goToRegistrationPageCommand ??
                    (_goToRegistrationPageCommand = new RelayCommand(obj =>
                    {
                        NavigationService.NavigateTo<RegistrationPageViewModel>();
                    }));
            }
        }
    }
}
