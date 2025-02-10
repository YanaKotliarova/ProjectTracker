using ProjectTracker.MVVM.Model;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithIssueService
    {
        Issue SelectedIssue { get; set; }

        Task<bool> ChechIssueNameAsync(int projectId, string name);
        ObservableCollection<Issue> CreateCollection(List<Issue> list);
        Task CreateIssueAsync(string issueName, string description);
        Task DeleteIssueAsync();
        List<Issue> GetAllUserIssues();
        List<Issue> GetIssuesList(int projectId, string status);
        List<Issue> GetProjectIssuesList();
        Task UpdateIssueInfoAsync();
    }
}