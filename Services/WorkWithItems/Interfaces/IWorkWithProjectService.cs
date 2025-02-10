
using ProjectTracker.MVVM.Model;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithProjectService
    {
        Project? SelectedProject { get; set; }

        Task<bool> CheckProjectNameAsync(string name);
        ObservableCollection<Project> CreateCollection();
        Task CreateProjectAsync(string projectName, string description);
        Task DeleteProjectAsync();
        string GetProjectName(int id);
        List<Project> GetUserProjectsList();
        Task UpdateProjectInfoAsync();
    }
}