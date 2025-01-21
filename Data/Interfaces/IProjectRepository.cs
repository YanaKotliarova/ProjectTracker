using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IProjectRepository : IDisposable
    {
        Task CreateAsync(Project newProject);
        Task DeleteAsync(int id);
        Task<Project> GetAsync(string projectName);
        Task<int> GetProjectId(string projectName);
        IEnumerable<Project> GetUserProjectsList(int userId);
        Task UpdateAsync(Project project);
    }
}
