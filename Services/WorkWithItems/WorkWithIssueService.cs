using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.WorkWithItems.Interfaces;

namespace ProjectTracker.Services.WorkWithItems
{
    public class WorkWithIssueService : IWorkWithIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkWithProjectService _workWithProject;

        public WorkWithIssueService(IIssueRepository issueRepository, IWorkWithProjectService workWithProject)
        {
            _issueRepository = issueRepository;
            _workWithProject = workWithProject;
        }

        public Issue SelectedIssue { get; set; }

        public async Task CreateIssueAsync(string issueName, string description)
        {
            await _issueRepository.CreateAsync(new Issue(_workWithProject.SelectedProject!.Id, issueName, description));
        }

        public async Task<List<Issue>> GetProjectIssuesListAsync()
        {
            List<Issue> issues = new List<Issue>();
            await foreach (List<Issue> listOfIssues in _issueRepository.GetProjectIssuesAsync(_workWithProject.SelectedProject!.Id))
            {
                if (listOfIssues.Count > 0)
                    issues.AddRange(listOfIssues);
            }
            return issues;
        }

        public async Task<List<Issue>> GetIssuesByStatusAsync(int projectId, string status)
        {
            List<Issue> issues = new List<Issue>();
            await foreach (List<Issue> listOfIssues in _issueRepository.GetUserIssuesByStatusAsync(projectId, status))
            {
                if (listOfIssues.Count > 0)
                {
                    foreach (Issue issue in listOfIssues)
                    {
                        issue.ProjectName = await _workWithProject.GetProjectNameAsync(issue.ProjectId);
                    }
                    issues.AddRange(listOfIssues);
                }
                    
            }
            return issues;
        }

        public async Task<List<Issue>> GetAllUserIssuesAsync()
        {
            List<Issue> allIssuesList = new List<Issue>();

            foreach (Project p in await _workWithProject.GetUserProjectsListAsync())
            {
                await foreach (List<Issue> listOfIssues in _issueRepository.GetProjectIssuesAsync(p.Id))
                {
                    if (listOfIssues.Count > 0)
                    {
                        foreach (Issue issue in listOfIssues)
                        {
                            issue.ProjectName = await _workWithProject.GetProjectNameAsync(issue.ProjectId);
                        }
                        allIssuesList.AddRange(listOfIssues);
                    }
                }
            }
            return allIssuesList;
        }

        public async Task<bool> ChechIssueNameAsync(int projectId, string name)
        {
            if ((SelectedIssue != null) && projectId.Equals(SelectedIssue.ProjectId) && name.Equals(SelectedIssue.Name))
                return false;
            else return await _issueRepository.GetByNameAsync(projectId, name) != null;
        }

        public async Task UpdateIssueInfoAsync()
        {
            await _issueRepository.UpdateAsync(SelectedIssue);
        }

        public async Task DeleteIssueAsync()
        {
            await _issueRepository.DeleteAsync(SelectedIssue.Id);
        }
    }
}
