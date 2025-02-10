using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
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
    }
}
