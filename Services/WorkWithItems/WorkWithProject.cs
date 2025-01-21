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
        public async Task CreateProjectAsync(string projectName, string? description)
        {
            Project newProject = new Project(_account.CurrentUser.Id, projectName, description);
            await _projectRepository.CreateAsync(newProject);
        }

        public List<string> GetUserProjectsNames()
        {
            List<string> projectsNames = new List<string>();

            foreach (var p in _projectRepository.GetUserProjectsList(_account.CurrentUser.Id))
                projectsNames.Add(p.Name);

            return projectsNames;
        }
        public List<Project> GetUserProjects()
        {
            return _projectRepository.GetUserProjectsList(_account.CurrentUser.Id).ToList();
        }

        public ObservableCollection<T> CreateCollection<T>(List<T> list)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            foreach (T p in list)
                collection.Add(p);
            return collection;
        }
        
    }
}
