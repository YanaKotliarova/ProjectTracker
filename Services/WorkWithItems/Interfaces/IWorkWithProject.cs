
using ProjectTracker.MVVM.Model;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithProject
    {
        ObservableCollection<T> CreateCollection<T>(List<T> list);
        Task CreateProjectAsync(string projectName, string description);
        List<Project> GetUserProjects();
        List<string> GetUserProjectsNames();
    }
}