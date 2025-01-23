using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data
{
    public class IssueRepository : IIssueRepository
    {
        private readonly ApplicationContext _db;
        private readonly IRepository _repository;
        public IssueRepository(IRepository repository)
        {
            _repository = repository;
            _db = _repository.GetDB();
        }
        public async Task CreateAsync(Issue newIssue)
        {
            await _db.Issues.AddAsync(newIssue);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Issue issue = await _db.Issues.FindAsync(id);
            if (issue != null) _db.Issues.Remove(issue);
            await _repository.SaveChangesAsync();
        }

        public async Task<Issue> GetAsync(int id)
        {
            return await _db.Issues.FindAsync(id);
        }

        public IEnumerable<Issue> GetProjectIssues(int projectId)
        {
            return _db.Issues.Where(i => i.ProjectId == projectId);
        }

        public async Task UpdateAsync(Issue issue)
        {
            _db.Entry(issue).State = EntityState.Modified;
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
