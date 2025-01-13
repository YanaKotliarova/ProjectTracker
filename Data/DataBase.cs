using ProjectTracker.Data.Interfaces;

namespace ProjectTracker.Data
{
    public class DataBase : IRepository
    {
        private readonly ApplicationContext db;
        public DataBase()
        {
            db = new ApplicationContext();
        }

        public ApplicationContext GetDB()
        {
            return db;
        }

        public async Task InitializeDBAsync()
        {
            //await db.Database.EnsureCreatedAsync();
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
