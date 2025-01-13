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
            _db = _repository.GetDB();
        }
        public async Task CreateAsync(Project newProject)
        {
            await _db.Projects.AddAsync(newProject);
        }

        public async Task DeleteAsync(int id)
        {
            Project project = await _db.Projects.FindAsync(id);
            if (project != null) _db.Projects.Remove(project);
        }

        public async Task<Project> GetAsync(int id)
        {
            return await _db.Projects.FindAsync(id);
        }

        public async Task UpdateAsync(Project project)
        {
            _db.Entry(project).State = EntityState.Modified;
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
