using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private const int AmountOfProjectForSelection = 10;

        private readonly ApplicationContext _db;
        public ProjectRepository(ApplicationContext applicationContext)
        {
            _db = applicationContext;
        }
        public async Task CreateAsync(Project newProject)
        {
            await _db.Projects.AddAsync(newProject);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Project project = await _db.Projects.FindAsync(id);
            if (project != null) _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        public async Task<Project> GetAsync(int id)
        {
            return await _db.Projects.FindAsync(id);
        }

        public async Task<Project> GetByNameAsync(string name)
        {
            return await _db.Projects.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async IAsyncEnumerable<List<Project>> GetUserProjectsAsync(int userId)
        {
            List<Project> listOfUserProjects = new List<Project>();
            int amountOfProjectsInDb = await _db.Projects.CountAsync();
            int amountOfViewProjects = 0;

            while (amountOfViewProjects < amountOfProjectsInDb)
            {
                //здесь
                listOfUserProjects.AddRange(await 
                    (from project in _db.Projects.Include(d => d.Issues).Skip(amountOfViewProjects).Take(AmountOfProjectForSelection)
                     where project.UserId.Equals(userId)
                     select project).ToListAsync());

                amountOfViewProjects += AmountOfProjectForSelection;

                if (listOfUserProjects.Count >= AmountOfProjectForSelection || amountOfViewProjects >= amountOfProjectsInDb)
                {
                    yield return listOfUserProjects;
                    listOfUserProjects.Clear();
                }
            }           
        }

        public async Task UpdateAsync(Project project)
        {
            _db.Entry(project).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
