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

        /// <summary>
        /// The method for adding new project to database.
        /// </summary>
        /// <param name="newProject"> Project object for creating. </param>
        /// <returns></returns>
        public async Task CreateAsync(Project newProject)
        {
            await _db.Projects.AddAsync(newProject);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for deleting project from database by id.
        /// </summary>
        /// <param name="id"> Id of project for deleting. </param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            Project project = await _db.Projects.FindAsync(id);
            if (project != null) _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for getting project from database by id.
        /// </summary>
        /// <param name="id"> Id of project for searching. </param>
        /// <returns> Required project or null. </returns>
        public async Task<Project> GetAsync(int id)
        {
            return await _db.Projects.FindAsync(id);
        }

        /// <summary>
        /// The method for getting project from database by its name.
        /// </summary>
        /// <param name="name"> Name of project for searching. </param>
        /// <returns> Required project or null. </returns>
        public async Task<Project> GetByNameAsync(string name)
        {
            return await _db.Projects.FirstOrDefaultAsync(p => p.Name == name);
        }

        /// <summary>
        /// The method for getting all projects of user from database by user id.
        /// </summary>
        /// <param name="userId"> Id of user for selection. </param>
        /// <returns> Lists with the user's projects, [AmountOfProjectForSelection] projects each. </returns>
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

        /// <summary>
        /// The method for update project information in database.
        /// </summary>
        /// <param name="project"> Project object with updated information. </param>
        /// <returns></returns>
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
