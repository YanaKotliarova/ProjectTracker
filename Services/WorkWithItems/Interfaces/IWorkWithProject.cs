
using ProjectTracker.MVVM.Model;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithProject
    {
        Project? SelectedProject { get; set; }

        Task<bool> CheckProjectNameAsync(string name);
        ObservableCollection<Project> CreateCollection();
        Task CreateProjectAsync(string projectName, string description);
        Task DeleteProject();
        string GetProjectName(int id);
        List<Project> GetUserProjectsList();
        Task UpdateProjectInfo();
    }
}