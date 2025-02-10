using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IProjectRepository : IDisposable
    {
        Task CreateAsync(Project newProject);
        Task DeleteAsync(int id);
        Task<Project> GetAsync(int id);
        Task<Project> GetByNameAsync(string name);
        IEnumerable<Project> GetUserProjects(int userId);
        Task UpdateAsync(Project project);
    }
}
