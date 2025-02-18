using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectTracker.Data.Interfaces;

namespace ProjectTracker.Data
{
    public class DataBase : IRepository
    {
        private readonly ApplicationContext _db;
        private readonly IConnectionStringValidation _connectionStringValidation;
        public DataBase(IConnectionStringValidation connectionStringValidation, ApplicationContext applicationContext)
        {
            _db = applicationContext;
            _connectionStringValidation = connectionStringValidation;
        }

        /// <summary>
        /// The method for initializing database if it not exists. 
        /// Check connectiong string and throws an exception if it incorrect.
        /// </summary>
        /// <param name="connectionString"> (Optional) Connection string for connection with database. </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InitializeDbAsync(string connectionString = null)
        {
            if (!connectionString.IsNullOrEmpty())
                _db.ConnectionString = connectionString;

            if (!_connectionStringValidation.ValidateConnectionString(_db.ConnectionString))
                throw new Exception(Properties.Resources.ConnectionStringExeption);

            await _db.Database.MigrateAsync();
        }
    }
}
