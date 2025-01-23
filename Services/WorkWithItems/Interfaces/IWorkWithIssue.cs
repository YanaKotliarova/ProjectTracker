using ProjectTracker.MVVM.Model;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithIssue
    {
        ObservableCollection<Issue> CreateCollection();
        Task CreateIssueAsync(string issueName, string description);
        List<Issue> GetProjectIssuesList();
    }
}