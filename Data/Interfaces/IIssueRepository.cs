using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IIssueRepository : IDisposable
    {
        Task CreateAsync(Issue newIssue);
        Task DeleteAsync(int id);
        Task<Issue> GetAsync(int id);
        IEnumerable<Issue> GetProjectIssues(int projectId);
        Task UpdateAsync(Issue issue);
    }
}
