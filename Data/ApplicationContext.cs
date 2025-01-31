using Microsoft.EntityFrameworkCore;
using ProjectTracker.MVVM.Model;
using System.Configuration;

namespace ProjectTracker.Data
{
    public class ApplicationContext : DbContext
    {
        private const string DefaultConnection = "DefaultConnection";
        internal DbSet<User> Users { get; set; } = null!;
        internal DbSet<Project> Projects { get; set; } = null!;
        internal DbSet<Issue> Issues { get; set; } = null!;

        internal string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;

        /// <summary>
        /// The method of connecting to the DB.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
