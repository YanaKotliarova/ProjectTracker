using ControlzEx.Standard;
using ProjectTracker.Data;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems
{
    public class WorkWithIssue : IWorkWithIssue
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkWithProject _workWithProject;

        public WorkWithIssue(IIssueRepository issueRepository, IWorkWithProject workWithProject)
        {
            _issueRepository = issueRepository;
            _workWithProject = workWithProject;
        }

        public Issue SelectedIssue { get; set; }

        public async Task CreateIssueAsync(string issueName, string description)
        {
            await _issueRepository.CreateAsync(new Issue(_workWithProject.SelectedProject!.Id, issueName, description));
        }

        public List<Issue> GetProjectIssuesList()
        {
            return _issueRepository.GetProjectIssues(_workWithProject.SelectedProject!.Id).ToList();
        }

        public List<Issue> GetIssuesList(int projectId, string status)
        {
            return _issueRepository.GetUserIssuesByStatus(projectId, status).ToList();
        }

        public List<Issue> GetAllUserIssues()
        {
            List<Issue> allIssues = new List<Issue>();
            foreach (var p in _workWithProject.GetUserProjectsList())
            {
                allIssues.AddRange(_issueRepository.GetProjectIssues(p.Id));
            }
            return allIssues;
        }

        public async Task<bool> ChechIssueNameAsync(int projectId, string name)
        {
            if ((SelectedIssue != null) && projectId.Equals(SelectedIssue.ProjectId) && name.Equals(SelectedIssue.Name))
                    return false;
            else return await _issueRepository.GetByNameAsync(projectId, name) != null;
        }

        public ObservableCollection<Issue> CreateCollection(List<Issue> list)
        {
            ObservableCollection<Issue> collection = new ObservableCollection<Issue>();
            foreach (Issue i in list)
                collection.Add(i);
            return collection;
        }

        public async Task UpdateIssueInfo()
        {
            await _issueRepository.UpdateAsync(SelectedIssue);
        }

        public async Task DeleteIssue()
        {
            await _issueRepository.DeleteAsync(SelectedIssue.Id);
        }
    }
}
