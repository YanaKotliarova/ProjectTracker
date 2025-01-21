using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        public MainWindowViewModel(INavigationService navigationService, IRepository repository)
        {
            NavigationService = navigationService;
            _repository = repository;
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

        private RelayCommand _navigateToAutorizationCommand;
        public RelayCommand NavigateToAutorizationCommand
        {
            get
            {
                return _navigateToAutorizationCommand ??
                    (_navigateToAutorizationCommand = new RelayCommand(async obj =>
                    {
                        await Task.Run(async () => await _repository.InitializeDBAsync());
                        NavigationService.NavigateTo<AutorizationPageViewModel>();
                    }));
            }
        }
    }
}
