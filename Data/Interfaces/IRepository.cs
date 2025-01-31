namespace ProjectTracker.Data.Interfaces
{
    public interface IRepository
    {
        ApplicationContext GetDb();
        Task InitializeDbAsync(string connectionString = null);
        Task SaveChangesAsync();
    }
}