using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;
using Dapper;

namespace ConsoleApp1.Data.Repositories
{
    public class MediaRepository(IDbConnectionFactory db) : IMediaRepository
    {
        private readonly IDbConnectionFactory _db = db;

        public async Task<IEnumerable<Media>> GetAllMediasAsync()
        {
            using var connection = await _db.GetOpenConnectionInterface();
            return await connection.QueryAsync<Media>("SELECT * FROM Media");
        }

        public async Task<Media?> GetMediaByIdAsync(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            return await connection.QueryFirstOrDefaultAsync<Media>(
                "SELECT * FROM Media WHERE ID = @Id", new { Id = id });
        }

        public async Task<int> CreateMediaAsync(Media media)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = @"INSERT INTO Media (Name, GroupID, Type, Status, Path) 
                        VALUES (@Name, @GroupID, @Type, @Status, @Path);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, media);
        }

        public async Task<int> UpdateMediaAsync(Media media)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            var sql = @"UPDATE Media 
                        SET Name = @Name, GroupID = @GroupID, Type = @Type, Status = @Status
                        WHERE ID = @ID";

            if (media.Path is not null)
            {
                sql = @"UPDATE Media 
                        SET Name = @Name, GroupID = @GroupID, Type = @Type, Status = @Status, Path = @Path
                        WHERE ID = @ID";
            }
            return await connection.ExecuteAsync(sql, media);
        }

        public async Task<int> DeleteMediaAsync(int id)
        {
            using var connection = await _db.GetOpenConnectionInterface();
            return await connection.ExecuteAsync("DELETE FROM Media WHERE ID = @Id", new { Id = id });
        }


    }
}