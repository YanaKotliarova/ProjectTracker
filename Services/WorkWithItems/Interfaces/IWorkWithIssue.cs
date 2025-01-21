namespace ProjectTracker.Services.WorkWithItems.Interfaces
{
    public interface IWorkWithIssue
    {
        Task CreateIssueAsync(string issueName, string description, string selectedProject);
    }
}