using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data
{
    public class IssueRepository : IIssueRepository
    {
        private const int AmountOfIssueForSelection = 100;

        private readonly ApplicationContext _db;
        public IssueRepository(ApplicationContext applicationContext)
        {
            _db = applicationContext;
        }
        public async Task CreateAsync(Issue newIssue)
        {
            await _db.Issues.AddAsync(newIssue);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Issue issue = await _db.Issues.FindAsync(id);
            if (issue != null) _db.Issues.Remove(issue);
            await _db.SaveChangesAsync();
        }

        public async Task<Issue> GetAsync(int id)
        {
            return await _db.Issues.FindAsync(id);
        }

        public async IAsyncEnumerable<List<Issue>> GetUserIssuesByStatusAsync(int projectId, string status)
        {
            List<Issue> listOfUserIssuesByStatus = new List<Issue>();
            int amountOfIssuesInDb = await _db.Issues.CountAsync();
            int amountOfViewIssues = 0;

            while (amountOfViewIssues < amountOfIssuesInDb)
            {
                listOfUserIssuesByStatus.AddRange(await
                    (from issue in _db.Issues.Skip(amountOfViewIssues).Take(AmountOfIssueForSelection)
                     where issue.ProjectId == projectId && issue.Status == status
                     select issue).ToListAsync());

                amountOfViewIssues += AmountOfIssueForSelection;

                if (listOfUserIssuesByStatus.Count > AmountOfIssueForSelection || amountOfViewIssues >= amountOfIssuesInDb)
                {
                    yield return listOfUserIssuesByStatus;
                    listOfUserIssuesByStatus.Clear();
                }
            }
        }

        public async Task<Issue> GetByNameAsync(int projectId, string name)
        {
            return await _db.Issues.FirstOrDefaultAsync(i => i.ProjectId == projectId && i.Name == name);
        }

        public async IAsyncEnumerable<List<Issue>> GetProjectIssuesAsync(int projectId)
        {
            List<Issue> listOfUserIssuesByStatus = new List<Issue>();
            int amountOfIssuesInDb = await _db.Issues.CountAsync();
            int amountOfViewIssues = 0;

            while (amountOfViewIssues < amountOfIssuesInDb)
            {
                //здесь
                listOfUserIssuesByStatus.AddRange(await
                    (from issue in _db.Issues.Skip(amountOfViewIssues).Take(AmountOfIssueForSelection)
                     where issue.ProjectId == projectId
                     select issue).ToListAsync());

                amountOfViewIssues += AmountOfIssueForSelection;

                if (listOfUserIssuesByStatus.Count > AmountOfIssueForSelection || amountOfViewIssues >= amountOfIssuesInDb)
                {
                    yield return listOfUserIssuesByStatus;
                    listOfUserIssuesByStatus.Clear();
                }
            }
        }

        public async Task UpdateAsync(Issue issue)
        {
            _db.Entry(issue).State = EntityState.Modified;
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
