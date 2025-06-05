using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;
using Dapper;

namespace ConsoleApp1.Data.Repositories
{
    public class GroupRepository(IDbConnectionFactory db) : IGroupRepository
    {
        private readonly IDbConnectionFactory _db = db;

        public async Task<int> CreateGroup(Group group)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "INSERT INTO [Group] (Name) VALUES (@Name); SELECT CAST(scope_identity() AS int)";
            return await connection.ExecuteScalarAsync<int>(sql, group);
        }

        public async Task<int> DeleteGroup(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sqlMedia = "DELETE FROM [Media] WHERE GroupID = @Id";
            var sql = "DELETE FROM [Group] WHERE ID = @Id";

            using (var transaction = connection.BeginTransaction())
            {
                int mediaDeleted = await connection.ExecuteAsync(sqlMedia, new { Id = id }, transaction);

                int groupDeleted = await connection.ExecuteAsync(sql, new { Id = id }, transaction);

                transaction.Commit();

                return groupDeleted;
            }
        }

        public async Task<IEnumerable<Group>> GetAllGroups(PaginationParams query)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = @"SELECT * FROM [Group] g LEFT JOIN Media m ON g.ID = m.GroupID 
            WHERE (@Search IS NULL OR g.Name LIKE '%' + @Search + '%') 
            ORDER BY g.Name 
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var groupDictionary = new Dictionary<int, Group>();

            var result = await connection.QueryAsync<Group, Media, Group>(sql, (group, media) =>
            {
                if (!groupDictionary.TryGetValue(group.Id, out var groupEntry))
                {
                    groupEntry = group;
                    groupEntry.Medias = [];
                    groupDictionary.Add(group.Id, groupEntry);
                }
                if (media is not null && media.ID != 0)
                    groupEntry.Medias.Add(media);

                return groupEntry;
            }, new
            {
                Search = query.Search,
                Offset = (query.PageNumber - 1) * query.PageSize,
                PageSize = query.PageSize
            }, splitOn: "ID"
            );

            return result.Distinct();
        }

        public async Task<Group?> GetGroupById(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "SELECT * FROM [Group] WHERE ID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Group>(sql, new { Id = id });
        }

        public async Task<Group?> GetGroupWithMedias(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "SELECT * FROM [Group] g LEFT JOIN Media m ON g.ID = m.GroupID WHERE g.ID = @Id";
            var groupDictionary = new Dictionary<int, Group>();

            var result = await connection.QueryAsync<Group, Media, Group>(sql, (group, media) =>
            {
                if (!groupDictionary.TryGetValue(group.Id, out var groupEntry))
                {
                    groupEntry = group;
                    groupEntry.Medias = [];
                    groupDictionary.Add(group.Id, groupEntry);
                }
                if (media is not null && media.ID != 0)
                    groupEntry.Medias.Add(media);

                return groupEntry;
            }, new { Id = id }, splitOn: "ID"
            );

            return result.FirstOrDefault();
        }

        public async Task<int> UpdateGroup(Group group)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = "UPDATE [Group] SET Name = @Name WHERE ID = @Id";
            return await connection.ExecuteAsync(sql, group);
        }
    }
}