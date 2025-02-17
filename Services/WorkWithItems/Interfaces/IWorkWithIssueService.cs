using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithIssueService
    {
        Issue SelectedIssue { get; set; }
        Task<bool> ChechIssueNameAsync(int projectId, string name);
        Task CreateIssueAsync(string issueName, string description);
        Task DeleteIssueAsync();
        Task<List<Issue>> GetAllUserIssuesAsync();
        Task<List<Issue>> GetIssuesByStatusAsync(int projectId, string status);
        Task<List<Issue>> GetProjectIssuesListAsync();
        Task UpdateIssueInfoAsync();
    }
}