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

        /// <summary>
        /// A property to store the selected issue.
        /// </summary>
        public Issue SelectedIssue { get; set; }

        /// <summary>
        /// The method for create new issue.
        /// </summary>
        /// <param name="issueName"> Name of created issue. </param>
        /// <param name="description"> Description of created issue. </param>
        /// <returns></returns>
        public async Task CreateIssueAsync(string issueName, string description)
        {
            await _issueRepository.CreateAsync(new Issue(_workWithProject.SelectedProject!.Id, issueName, description));
        }

        /// <summary>
        /// The method for getting issues by status.
        /// </summary>
        /// <param name="projectId"> Id of project. </param>
        /// <param name="status"> Required status. </param>
        /// <returns> List of issue. </returns>
        public async Task<List<Issue>> GetIssuesByStatusAsync(int projectId, string status)
        {
            List<Issue> issues = new List<Issue>();
            await foreach (List<Issue> listOfIssues in _issueRepository.GetIssuesAsync(projectId, status))
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

        /// <summary>
        /// The method for getting all user issues.
        /// </summary>
        /// <returns> List of all user issues. </returns>
        public async Task<List<Issue>> GetAllUserIssuesAsync()
        {
            List<Issue> allIssuesList = new List<Issue>();

            foreach (Project p in await _workWithProject.GetUserProjectsListAsync())
            {
                await foreach (List<Issue> listOfIssues in _issueRepository.GetIssuesAsync(p.Id))
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

        /// <summary>
        /// The method for checking if entered project name exists its project in database.
        /// </summary>
        /// <param name="projectId"> Id of issue project. </param>
        /// <param name="name"> Name of issue. </param>
        /// <returns></returns>
        public async Task<bool> ChechIssueNameAsync(int projectId, string name)
        {
            if ((SelectedIssue != null) && projectId.Equals(SelectedIssue.ProjectId) && name.Equals(SelectedIssue.Name))
                return false;
            else return await _issueRepository.GetByNameAsync(projectId, name) != null;
        }

        /// <summary>
        /// The method for updating issue information.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateIssueInfoAsync()
        {
            await _issueRepository.UpdateAsync(SelectedIssue);
        }

        /// <summary>
        /// The method for deleting issue.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteIssueAsync()
        {
            await _issueRepository.DeleteAsync(SelectedIssue.Id);
        }
    }
}
