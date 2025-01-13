using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        public RegistrationPageViewModel(INavigationService navigationService)
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
    }
}
