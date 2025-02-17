using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Localization;
using ProjectTracker.Services.Navigation.Interfaces;
using ProjectTracker.Services.ServiceHelpers.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AutorizationPageViewModel : ViewModelBase
    {
        private readonly IAutorizationService _autorization;
        private readonly ICollectionHelper _collectionHelper;
        public AutorizationPageViewModel(INavigationService navigationService, IAutorizationService autorization, 
            ICollectionHelper collectionHelper)
        {
            NavigationService = navigationService;
            _autorization = autorization;
            _collectionHelper = collectionHelper;

            Languages = _collectionHelper.CreateCollection(TranslationSource.Instance.GetLanguages());
            Language = TranslationSource.Instance.SetLanguage();
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

        private ObservableCollection<string> _languages = new ObservableCollection<string>();
        /// <summary>
        /// A property for creating collection of languages that the application has been translated into.
        /// </summary>
        public ObservableCollection<string> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        private string _language;
        /// <summary>
        /// A property associated with SelectedItem property of ComboBox for choosing language.
        /// Changes the language of the application according to the selected item of ComboBox.
        /// </summary>
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
            }
        }

        private RelayCommand _languageChangedCommand;
        public RelayCommand LanguageChangedCommand
        {
            get
            {
                return _languageChangedCommand ??
                    (_languageChangedCommand = new RelayCommand(obj =>
                    {
                        ChangeLanguage(Language);
                    }));
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

        private static void ChangeLanguage(string locale)
        {
            if (string.IsNullOrEmpty(locale)) locale = "en";
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(locale);
        }
    }
}
