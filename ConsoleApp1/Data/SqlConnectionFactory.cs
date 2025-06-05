using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1.Data
{
    public class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        private readonly string _connectionString = connectionString;

        public async Task<IDbConnection> GetOpenConnectionInterface()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}