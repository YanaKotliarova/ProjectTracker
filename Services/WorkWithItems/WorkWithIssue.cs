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
        public async Task CreateIssueAsync(string issueName, string description)
        {
            await _issueRepository.CreateAsync(new Issue(_workWithProject.SelectedProject!.Id, issueName, description));
        }

        public List<Issue> GetProjectIssuesList()
        {
            return _issueRepository.GetProjectIssues(_workWithProject.SelectedProject!.Id).ToList();
        }

        public ObservableCollection<Issue> CreateCollection()
        {
            ObservableCollection<Issue> collection = new ObservableCollection<Issue>();
            foreach (Issue i in GetProjectIssuesList())
                collection.Add(i);
            return collection;
        }
    }
}
