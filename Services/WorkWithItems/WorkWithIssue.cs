using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.WorkWithItems.Interfaces;

namespace ProjectTracker.Services.WorkWithItems
{
    public class WorkWithIssue : IWorkWithIssue
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IProjectRepository _projectRepository;

        public WorkWithIssue(IIssueRepository issueRepository, IProjectRepository projectRepository)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
        }
        public async Task CreateIssueAsync(string issueName, string description, string selectedProject)
        {
            await _issueRepository.CreateAsync(
                new Issue(await _projectRepository.GetProjectId(selectedProject), issueName, description));
        }
    }
}
