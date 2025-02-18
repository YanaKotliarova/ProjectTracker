using ProjectTracker.MVVM.Core;
using ProjectTracker.Services.Navigation.Interfaces;

namespace ProjectTracker.Services.Navigation
{
    public class NavigationService : ViewModelBase, INavigationService
    {
        private readonly Func<Type, ViewModelBase> _viewModelFactory;
        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        private ViewModelBase _currentViewModel;
        /// <summary>
        /// A property for storing the current ViewModel.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        /// <summary>
        /// The method for openin view from getting its type from current ViewModel.
        /// The types are recorded in the application resources.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentViewModel = viewModel;
        }
    }
}
