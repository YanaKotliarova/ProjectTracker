using Microsoft.Data.SqlClient;
using ProjectTracker.Data.Interfaces;

namespace ProjectTracker.Data
{
    public class ConnectionStringValidation : IConnectionStringValidation
    {
        /// <summary>
        /// The method for checking correctness of connection string.
        /// </summary>
        /// <param name="connectionString"> Connection string for cheking. </param>
        /// <returns> True if connection string is correct, otherwise false. </returns>
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
