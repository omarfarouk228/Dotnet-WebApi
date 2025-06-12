using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;
using Dapper;

namespace ConsoleApp1.Data.Repositories
{
    public class FolderRepository(IDbConnectionFactory db) : IFolderRepository
    {
        private readonly IDbConnectionFactory _db = db;

        public async Task<int> CreateFolder(FolderCreate folder)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "INSERT INTO [Folders] (Name, ParentId) VALUES (@Name, @ParentId); SELECT CAST(scope_identity() AS int)";
            return await connection.ExecuteScalarAsync<int>(sql, folder);
        }

        public async Task<int> DeleteFolder(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            using var transaction = connection.BeginTransaction();

            int deleted = await DeleteFolderRecursive(id, connection, transaction);
            transaction.Commit();
            return deleted;

        }

        public async Task<int> DeleteFolderRecursive(int id, System.Data.IDbConnection connection, System.Data.IDbTransaction transaction)
        {
            int totalDeleted = 0;
            var sqlFile = "DELETE FROM [Files] WHERE FolderId = @Id";
            var sql = "DELETE FROM [Folders] WHERE Id = @Id";
            var sqlFolders = "SELECT * FROM [Folders] WHERE ParentId = @Id";

            // Supprimer récursivement les sous-dossiers
            var childrens = await connection.QueryAsync<Folder>(sqlFolders, new { Id = id }, transaction);
            foreach (var child in childrens)
            {
                totalDeleted += await DeleteFolderRecursive(child.Id, connection, transaction);
            }

            // Supprimer les fichiers
            totalDeleted += await connection.ExecuteAsync(sqlFile, new { Id = id }, transaction);

            // Supprimer le dossier
            totalDeleted += await connection.ExecuteAsync(sql, new { Id = id }, transaction);

            return totalDeleted;
        }


        public async Task<IEnumerable<Folder>> GetAllFolders()
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = @"SELECT * FROM [Folders] WHERE ParentId IS NULL";

            return await connection.QueryAsync<Folder>(sql);
        }


        public async Task<IEnumerable<Folder>> GetChildrens(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = @"SELECT * FROM [Folders] WHERE ParentId = @ParentId";

            return await connection.QueryAsync<Folder>(sql, new { ParentId = id });
        }

        public async Task<Folder?> GetFolderById(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "SELECT * FROM [Folders] WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Folder>(sql, new { Id = id });
        }

        public async Task<bool> HasFiles(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "SELECT COUNT(1) FROM [Files] WHERE FolderId = @Id";
            return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { Id = id });
        }

        public async Task<int> UpdateFolder(FolderUpdate folder)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "UPDATE [Folders] SET Name = @Name, ParentId = @ParentId WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, folder);
        }
    }
}