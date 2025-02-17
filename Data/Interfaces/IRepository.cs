namespace ProjectTracker.Data.Interfaces
{
    public interface IRepository
    {
        Task InitializeDbAsync(string connectionString = null);
    }
}