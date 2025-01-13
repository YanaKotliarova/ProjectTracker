using ProjectTracker.MVVM.Core;

namespace ProjectTracker.Services.Navigation.Interfaces
{
    public interface INavigationService
    {
        ViewModelBase CurrentViewModel { get; }
        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    }
}
