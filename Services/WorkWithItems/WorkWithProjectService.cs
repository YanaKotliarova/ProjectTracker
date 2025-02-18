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

        /// <summary>
        /// A property to store the selected project.
        /// </summary>
        public Project? SelectedProject { get; set; }

        /// <summary>
        /// The method for create new project.
        /// </summary>
        /// <param name="projectName"> Name of created project. </param>
        /// <param name="description"> Description of created project. </param>
        /// <returns></returns>
        public async Task CreateProjectAsync(string projectName, string? description)
        {
            await _projectRepository.CreateAsync(new Project(_account.CustomPrincipal.Identity.Id, projectName, description));
        }

        /// <summary>
        /// The method for getting all user projects.
        /// </summary>
        /// <returns> List of user projects. </returns>
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

        /// <summary>
        /// The method for getting name of project by id.
        /// </summary>
        /// <param name="id"> Id of required project. </param>
        /// <returns></returns>
        public async Task<string> GetProjectNameAsync(int id)
        {
            Project project = await _projectRepository.GetAsync(id);
            return project.Name;
        }

        /// <summary>
        /// The method for checking if entered project name exists in database.
        /// </summary>
        /// <param name="name"> Project name. </param>
        /// <returns> True if exists, otherwise false. </returns>
        public async Task<bool> CheckProjectNameAsync(string name)
        {
            if ((SelectedProject != null) && name.Equals(SelectedProject.Name))
                return false;
            else return await _projectRepository.GetByNameAsync(name) != null;
        }
        
        /// <summary>
        /// The method for updating project information.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateProjectInfoAsync()
        {
            await _projectRepository.UpdateAsync(SelectedProject!);
        }

        /// <summary>
        /// The method for deleting project.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteProjectAsync()
        {
            await _projectRepository.DeleteAsync(SelectedProject!.Id);
        }
    }
}
