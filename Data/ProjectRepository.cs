using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _db;
        private readonly IRepository _repository;
        public ProjectRepository(IRepository repository)
        {
            _repository = repository;
            _db = _repository.GetDb();
        }
        public async Task CreateAsync(Project newProject)
        {
            await _db.Projects.AddAsync(newProject);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Project project = await _db.Projects.FindAsync(id);
            if (project != null) _db.Projects.Remove(project);
            await _repository.SaveChangesAsync();
        }

        public Project Get(int id)
        {
            return _db.Projects.Find(id);
        }

        public async Task<Project> GetByNameAsync(string name)
        {
            return await _db.Projects.FirstOrDefaultAsync(p => p.Name == name);
        }

        public IEnumerable<Project> GetUserProjects(int userId)
        {
            return _db.Projects.Where(p => p.UserId == userId);
        }

        public async Task UpdateAsync(Project project)
        {
            _db.Entry(project).State = EntityState.Modified;
            await _repository.SaveChangesAsync();
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
