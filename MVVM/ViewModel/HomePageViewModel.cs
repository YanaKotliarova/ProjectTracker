using MahApps.Metro.Controls;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.UIHelpers;
using ProjectTracker.Properties;
using ProjectTracker.Services.Navigation.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;

namespace ProjectTracker.MVVM.ViewModel
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
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

        private RelayCommand _goToAccountPageCommand;
        public RelayCommand GoToAccountPageCommand
        {
            get
            {
                return _goToAccountPageCommand ??
                    (_goToAccountPageCommand = new RelayCommand(obj =>
                    {
                        NavigationService.NavigateTo<AccountPageViewModel>();
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
    }
}
