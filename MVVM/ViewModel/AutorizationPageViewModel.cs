using Microsoft.IdentityModel.Tokens;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.UIHelpers.Interfaces;
using ProjectTracker.Properties;
using ProjectTracker.Services.Authentication.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;

namespace ProjectTracker.MVVM.ViewModel
{
    public class AutorizationPageViewModel : ViewModelBase
    {
        private readonly IAutorizationService _autorization;
        private readonly IMainView _mainView;
        public AutorizationPageViewModel(INavigationService navigationService, IAutorizationService autorization, IMainView mainView)
        {
            NavigationService = navigationService;
            _autorization = autorization;
            _mainView = mainView;

            GetLanguages();
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

        private ObservableCollection<CultureInfo> _languages = new ObservableCollection<CultureInfo>();
        /// <summary>
        /// A property for creating collection of languages that the application has been translated into.
        /// </summary>
        public ObservableCollection<CultureInfo> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        private CultureInfo _language;
        /// <summary>
        /// A property associated with SelectedItem property of ComboBox for choosing language.
        /// Changes the language of the application according to the selected item of ComboBox.
        /// </summary>
        public CultureInfo Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));

                Thread.CurrentThread.CurrentUICulture = value;
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

        private RelayCommand _languageChangedCommand;
        public RelayCommand LanguageChangedCommand
        {
            get
            {
                return _languageChangedCommand ??
                    (_languageChangedCommand = new RelayCommand(obj =>
                    {
                        //_mainView.Frame.Content = null;
                        //_mainView.Frame.Content = NavigationService.CurrentViewModel;
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

        /// <summary>
        /// The method for adding available languages into created collection.
        /// </summary>
        private void GetLanguages()
        {
            ResourceManager rm = new ResourceManager(typeof(Resources));
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            foreach (CultureInfo culture in cultures)
            {
                ResourceSet rs = rm.GetResourceSet(culture, true, false);

                if (rs != null)
                {
                    if (culture.Equals(CultureInfo.InvariantCulture))
                    {
                        continue;
                    }
                    else
                    {
                        Languages.Add(culture);
                    }
                }
            }

            Language = SetLanguageChanger();
        }

        /// <summary>
        /// The method for setting default value of ComboBox with available languages.
        /// </summary>
        /// <returns></returns>
        private CultureInfo SetLanguageChanger()
        {
            CultureInfo currentCultureInfo;
            CultureInfo cultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            if (Languages.Contains(cultureInfo))
                currentCultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            else currentCultureInfo = new CultureInfo("en");

            return currentCultureInfo;
        }
    }
}
