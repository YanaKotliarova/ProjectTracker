namespace ProjectTracker.Data.Interfaces
{
    public interface IConnectionStringValidation
    {
        bool ValidateConnectionString(string connectionString);
    }
}