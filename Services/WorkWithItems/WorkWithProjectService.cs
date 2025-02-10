using ProjectTracker.Data;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.WorkWithItems.Interfaces;
using System.Collections.ObjectModel;

namespace ProjectTracker.Services.WorkWithItems
{
    public class WorkWithProjectService : IWorkWithProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAccountService _account;

        public WorkWithProjectService(IProjectRepository projectRepository, IAccountService account)
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

        public async Task<string> GetProjectName(int id)
        {
            Project project = await _projectRepository.GetAsync(id);
            return project.Name;
        }

        public async Task<bool> CheckProjectNameAsync(string name)
        {
            if ((SelectedProject != null) && name.Equals(SelectedProject.Name))
                return false;
            else return await _projectRepository.GetByNameAsync(name) != null;
        }

        public ObservableCollection<Project> CreateCollection()
        {
            ObservableCollection<Project> collection = new ObservableCollection<Project>();
            foreach (Project p in GetUserProjectsList())
                collection.Add(p);
            return collection;
        }
        
        public async Task UpdateProjectInfoAsync()
        {
            await _projectRepository.UpdateAsync(SelectedProject!);
        }

        public async Task DeleteProjectAsync()
        {
            await _projectRepository.DeleteAsync(SelectedProject!.Id);
        }
    }
}
