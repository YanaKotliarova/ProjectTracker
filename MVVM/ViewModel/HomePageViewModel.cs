using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Navigation.Interfaces;
using System.Collections.ObjectModel;


namespace ProjectTracker.MVVM.ViewModel
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            Languages.Add("en");
            Languages.Add("ru");
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

        private string _language = "en";
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
            }
        }
    }
}
