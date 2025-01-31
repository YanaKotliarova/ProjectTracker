using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.ViewModel;

namespace ProjectTracker.Data
{
    public class DataBase : IRepository
    {
        private readonly ApplicationContext db;
        private readonly IConnectionStringValidation _connectionStringValidation;
        public DataBase(IConnectionStringValidation connectionStringValidation)
        {
            db = new ApplicationContext();
            _connectionStringValidation = connectionStringValidation;
        }

        public ApplicationContext GetDb()
        {
            return db;
        }

        public async Task InitializeDbAsync(string connectionString = null)
        {
            if (!connectionString.IsNullOrEmpty())
                db.ConnectionString = connectionString;

            if (!_connectionStringValidation.ValidateConnectionString(db.ConnectionString))
                throw new Exception("It is impossible to connect to the database. Check the connection string.");

            await db.Database.MigrateAsync();
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
