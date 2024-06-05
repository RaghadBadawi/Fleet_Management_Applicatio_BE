using System.Data;
using Npgsql;

namespace TProject.Models
{
    public class DbContext
    {
        private readonly string connectionString;

        public DbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        
        public DataTable ExecuteQuery(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
        }

        
        public int ExecuteNonQuery(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
