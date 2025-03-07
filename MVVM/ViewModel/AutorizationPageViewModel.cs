﻿using Microsoft.IdentityModel.Tokens;
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

        private bool _isLoginUnsuccessful;
        /// <summary>
        /// A property for binding a result of cheking if log in is successful and an IsOpen property of certain Popup.
        /// </summary>
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
        /// <summary>
        /// The command that is called when selected language is changed and binded with SelectionChanged event of ComboBox.
        /// </summary>
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
        /// <summary>
        /// The command that is called when log in button is clicked.
        /// </summary>
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
        /// <summary>
        /// The command of navigation to the registration page which is binded with a hyperlink.
        /// </summary>
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
        /// The method for change language of application.
        /// </summary>
        /// <param name="locale"></param>
        private static void ChangeLanguage(string locale)
        {
            if (string.IsNullOrEmpty(locale)) locale = "en";
            TranslationSource.Instance.CurrentCulture = new CultureInfo(locale);
        }
    }
}
