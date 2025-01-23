
using ProjectTracker.MVVM.Model;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithProject
    {
        Project? SelectedProject { get; set; }

        ObservableCollection<Project> CreateCollection();
        Task CreateProjectAsync(string projectName, string description);
        Task DeleteProject();
        List<Project> GetUserProjectsList();
        Task UpdateProjectInfo();
    }
}