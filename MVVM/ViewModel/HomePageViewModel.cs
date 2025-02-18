using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Navigation.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.MVVM.ViewModel
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly HomeUserControlViewModel _homeUserControlViewModel;
        private readonly ProjectsBoardUserControlViewModel _projectsBoardUserControlViewModel;
        private readonly IssueBoardUserControlViewModel _issueBoardUserControlViewModel;
        private readonly AddItemUserControlViewModel _addItemUserControlViewModel;

        public HomePageViewModel(INavigationService navigationService, HomeUserControlViewModel homeUserControl, 
            ProjectsBoardUserControlViewModel projectsBoardUserControl, IssueBoardUserControlViewModel issueBoardUserControl,
            AddItemUserControlViewModel addItemUserControl)
        {
            NavigationService = navigationService;
            _homeUserControlViewModel = homeUserControl;
            _projectsBoardUserControlViewModel = projectsBoardUserControl;
            _issueBoardUserControlViewModel = issueBoardUserControl;
            _addItemUserControlViewModel = addItemUserControl;

            CreateTabs();
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

        private ObservableCollection<ViewModelBase> _tabViewModels;
        /// <summary>
        /// An observable collection of views for TabControl's items.
        /// </summary>
        public ObservableCollection<ViewModelBase> TabViewModels
        {
            get { return _tabViewModels; }
            set
            {
                _tabViewModels = value;
                OnPropertyChanged(nameof(TabViewModels));
            }
        }

        private ViewModelBase _selectedTabViewModel;
        /// <summary>
        /// A property for getting the selected view.
        /// </summary>
        public ViewModelBase SelectedTabViewModel
        {
            get { return _selectedTabViewModel; }
            set
            {
                _selectedTabViewModel = value;
                OnPropertyChanged(nameof(SelectedTabViewModel));
            }
        }

        private RelayCommand _goToAccountPageCommand;
        /// <summary>
        /// The command of navigation to the accout page which is binded with a account button.
        /// </summary>
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
        /// The method for creating a collection of TabItems.
        /// </summary>
        private void CreateTabs()
        {
            TabViewModels = new ObservableCollection<ViewModelBase>();

            TabViewModels.Add(_homeUserControlViewModel);
            TabViewModels.Add(_projectsBoardUserControlViewModel);
            TabViewModels.Add(_issueBoardUserControlViewModel);
            TabViewModels.Add(_addItemUserControlViewModel);

            SelectedTabViewModel = TabViewModels[0];
        }
    }
}
