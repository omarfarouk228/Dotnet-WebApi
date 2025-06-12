using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;
using Dapper;

namespace ConsoleApp1.Data.Repositories
{
    public class FileRepository(IDbConnectionFactory db) : IFileRepository
    {
        private readonly IDbConnectionFactory _db = db;

        public async Task<int> CreateFile(FileCreate file)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "INSERT INTO [Files] (Name, FolderId, Content) VALUES (@Name, @FolderId, @Content); SELECT CAST(scope_identity() AS int)";
            return await connection.ExecuteScalarAsync<int>(sql, file);
        }

        public async Task<int> DeleteFile(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "DELETE FROM [Files] WHERE Id = @Id";

            return await connection.ExecuteScalarAsync<int>(sql, new { id });
        }

        public async Task<Models.File?> GetFileById(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "SELECT * FROM [Files] WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Models.File>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Models.File>> GetFilesByFolderId(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = @"SELECT * FROM [Files] WHERE FolderId = @FolderId";

            return await connection.QueryAsync<Models.File>(sql, new { FolderId = id });
        }


        public async Task<int> UpdateFile(FileUpdate file)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "UPDATE [Files] SET Name = @Name, ParentId = @ParentId, Content = @Content WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, file);
        }
    }
}