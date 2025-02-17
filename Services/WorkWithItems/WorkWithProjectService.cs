using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication;
using ProjectTracker.Services.WorkWithItems.Interfaces;

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
            await _projectRepository.CreateAsync(new Project(_account.CustomPrincipal.Identity.Id, projectName, description));
        }

        public async Task<List<Project>> GetUserProjectsListAsync()
        {
            List<Project> projects = new List<Project>();
            await foreach (List<Project> listOfProjects in _projectRepository.GetUserProjectsAsync(_account.CustomPrincipal.Identity.Id))
            {
                if (listOfProjects.Count > 0)
                    projects.AddRange(listOfProjects);
            }
            return projects;
        }

        public async Task<string> GetProjectNameAsync(int id)
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
