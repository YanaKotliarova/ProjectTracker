namespace ProjectTracker.Data.Interfaces
{
    public interface IRepository
    {
        ApplicationContext GetDB();
        Task InitializeDBAsync();
        Task SaveChangesAsync();
    }
}