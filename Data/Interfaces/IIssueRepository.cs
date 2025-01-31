using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IIssueRepository : IDisposable
    {
        Task CreateAsync(Issue newIssue);
        Task DeleteAsync(int id);
        Task<Issue> GetAsync(int id);
        IEnumerable<Issue> GetUserIssuesByStatus(int projectId, string status);
        IEnumerable<Issue> GetProjectIssues(int projectId);
        Task UpdateAsync(Issue issue);
        Task<Issue> GetByNameAsync(int projectId, string name);
    }
}
