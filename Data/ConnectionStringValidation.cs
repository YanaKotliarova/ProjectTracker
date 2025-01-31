using Microsoft.Data.SqlClient;
using ProjectTracker.Data.Interfaces;

namespace ProjectTracker.Data
{
    public class ConnectionStringValidation : IConnectionStringValidation
    {
        public bool ValidateConnectionString(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
