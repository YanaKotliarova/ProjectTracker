using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IIssueRepository : IDisposable
    {
        Task CreateAsync(Issue newIssue);
        Task DeleteAsync(int id);
        Task<Issue> GetAsync(int id);
        Task UpdateAsync(Issue issue);
        Task<Issue> GetByNameAsync(int projectId, string name);
        IAsyncEnumerable<List<Issue>> GetUserIssuesByStatusAsync(int projectId, string status);
        IAsyncEnumerable<List<Issue>> GetProjectIssuesAsync(int projectId);
    }
}
