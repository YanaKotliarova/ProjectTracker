using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Core;
using ProjectTracker.MVVM.View.UI.Interfaces;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.MVVM.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly IMetroDialog _metroDialog;
        public MainWindowViewModel(INavigationService navigationService, IRepository repository, IMetroDialog metroDialog)
        {
            NavigationService = navigationService;
            _repository = repository;
            _metroDialog = metroDialog;
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

        private RelayCommand _navigateToAutorizationCommand;
        /// <summary>
        /// The command of navigation to the autorization page.
        /// </summary>
        public RelayCommand NavigateToAutorizationCommand
        {
            get
            {
                return _navigateToAutorizationCommand ??
                    (_navigateToAutorizationCommand = new RelayCommand(async obj =>
                    {
                        await Task.Run(async () =>
                        {
                            try
                            {
                                await Task.Run(async () => await _repository.InitializeDbAsync());
                                NavigationService.NavigateTo<AutorizationPageViewModel>();
                            }
                            catch (Exception ex)
                            {
                                await _metroDialog.ShowMessage(this, Properties.Resources.InvalidConnectionString, ex.Message);
                                NavigationService.NavigateTo<EnterConnectionStringViewModel>();
                            }
                        });                        
                    }));
            }
        }
    }
}
