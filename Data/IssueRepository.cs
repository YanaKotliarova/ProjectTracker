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

        /// <summary>
        /// The method for adding new issue to database.
        /// </summary>
        /// <param name="newIssue"> Issue object for creating. </param>
        /// <returns></returns>
        public async Task CreateAsync(Issue newIssue)
        {
            await _db.Issues.AddAsync(newIssue);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for deleting issue from database by id.
        /// </summary>
        /// <param name="id"> Id of issue for deleting. </param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            Issue issue = await _db.Issues.FindAsync(id);
            if (issue != null) _db.Issues.Remove(issue);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for getting issue from database by id.
        /// </summary>
        /// <param name="id"> Id of issue for searching. </param>
        /// <returns> Required issue or null. </returns>
        public async Task<Issue> GetAsync(int id)
        {
            return await _db.Issues.FindAsync(id);
        }

        /// <summary>
        /// The method for getting issue from database by its project id and name.
        /// </summary>
        /// <param name="projectId"> Project id of required issue. </param>
        /// <param name="name"> Issue name for searching. </param>
        /// <returns> Required issue or null. </returns>
        public async Task<Issue> GetByNameAsync(int projectId, string name)
        {
            return await _db.Issues.FirstOrDefaultAsync(i => i.ProjectId == projectId && i.Name == name);
        }

        /// <summary>
        /// The method for getting all issues from database by project id and status (if set).
        /// </summary>
        /// <param name="projectId"> Id of project for selection. </param>
        /// <param name="status"> (Optional) Status of issue for selection. </param>
        /// <returns> Lists with issues, [AmountOfIssueForSelection] issues each. </returns>
        public async IAsyncEnumerable<List<Issue>> GetIssuesAsync(int projectId, string status = "%")
        {
            List<Issue> listOfUserIssuesByStatus = new List<Issue>();
            int amountOfIssuesInDb = await _db.Issues.CountAsync();
            int amountOfViewIssues = 0;

            while (amountOfViewIssues < amountOfIssuesInDb)
            {
                listOfUserIssuesByStatus.AddRange(await
                    (from issue in _db.Issues.Skip(amountOfViewIssues).Take(AmountOfIssueForSelection)
                     where issue.ProjectId == projectId && EF.Functions.Like(issue.Status, status)
                     select issue).ToListAsync());

                amountOfViewIssues += AmountOfIssueForSelection;

                if (listOfUserIssuesByStatus.Count > AmountOfIssueForSelection || amountOfViewIssues >= amountOfIssuesInDb)
                {
                    yield return listOfUserIssuesByStatus;
                    listOfUserIssuesByStatus.Clear();
                }
            }
        }

        /// <summary>
        /// The method for update issue information in database.
        /// </summary>
        /// <param name="issue"> Issue object with updated information. </param>
        /// <returns></returns>
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
