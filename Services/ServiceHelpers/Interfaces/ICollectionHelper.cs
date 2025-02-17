using System.Collections.ObjectModel;

namespace ProjectTracker.Services.ServiceHelpers.Interfaces
{
    public interface ICollectionHelper
    {
        ObservableCollection<T> CreateCollection<T>(List<T> list);
    }
}