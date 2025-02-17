
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithProjectService
    {
        Project? SelectedProject { get; set; }
        Task<bool> CheckProjectNameAsync(string name);
        Task CreateProjectAsync(string projectName, string description);
        Task DeleteProjectAsync();
        Task<string> GetProjectNameAsync(int id);
        Task<List<Project>> GetUserProjectsListAsync();
        Task UpdateProjectInfoAsync();
    }
}