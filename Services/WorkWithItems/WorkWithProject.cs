using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems
{
    public class WorkWithProject : IWorkWithProject
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAccount _account;

        public WorkWithProject(IProjectRepository projectRepository, IAccount account)
        {
            _projectRepository = projectRepository;
            _account = account;
        }

        public Project? SelectedProject { get; set; }

        public async Task CreateProjectAsync(string projectName, string? description)
        {
            await _projectRepository.CreateAsync(new Project(_account.CurrentUser.Id, projectName, description));
        }
        public List<Project> GetUserProjectsList()
        {
            return _projectRepository.GetUserProjects(_account.CurrentUser.Id).ToList();
        }

        public ObservableCollection<Project> CreateCollection()
        {
            ObservableCollection<Project> collection = new ObservableCollection<Project>();
            foreach (Project p in GetUserProjectsList())
                collection.Add(p);
            return collection;
        }
        
        public async Task UpdateProjectInfo()
        {
            await _projectRepository.UpdateAsync(SelectedProject!);
        }

        public async Task DeleteProject()
        {
            await _projectRepository.DeleteAsync(SelectedProject!.Id);
        }
    }
}
