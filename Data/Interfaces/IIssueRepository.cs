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
        IAsyncEnumerable<List<Issue>> GetIssuesAsync(int projectId, string status = "%");
    }
}
