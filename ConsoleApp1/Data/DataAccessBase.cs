using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1.Data
{
    public class DataAccessBase(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public async Task<IDbConnection> GetOpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<bool> CanConnect()
        {
            try
            {
                // Using pour fermer et libérer la connexion après
                using var connection = await GetOpenConnection();
                var result = await connection.QueryFirstOrDefaultAsync<int>("SELECT 1");
                return result == 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur de connexion : {ex.Message}");
                return false;
            }
        }
    }
}