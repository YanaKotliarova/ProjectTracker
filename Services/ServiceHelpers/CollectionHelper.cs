using ProjectTracker.Services.ServiceHelpers.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.ServiceHelpers
{
    public class CollectionHelper : ICollectionHelper
    {
        /// <summary>
        /// The method for creating collection of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public ObservableCollection<T> CreateCollection<T>(List<T> list)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            foreach (T t in list)
                collection.Add(t);
            return collection;
        }
    }
}
