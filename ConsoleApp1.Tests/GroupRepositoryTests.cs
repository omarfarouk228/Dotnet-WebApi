using System.Data;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Data.Repositories;
using ConsoleApp1.Models;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;

namespace ConsoleApp1.Tests
{
    public class GroupRepositoryTests
    {
        [Fact]
        public async Task TestGetAllGroups()
        {
            var mockFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();


            var exceptedGroups = new List<Group>{
                new() {Id = 1, Name = "Group 1"},
                new() {Id = 2, Name = "Group 2"}
            };

            mockFactory.Setup(f => f.GetOpenConnectionInterface()).ReturnsAsync(mockConnection.Object);

            mockConnection
            .SetupDapperAsync(c => c.QueryAsync<Group>(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(exceptedGroups);


            var repository = new GroupRepository(mockFactory.Object);
            var result = await repository.GetAllGroups(new PaginationParams());

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task TestCreateGroup()
        {

            var mockFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();

            mockFactory.Setup(f => f.GetOpenConnectionInterface()).ReturnsAsync(mockConnection.Object);

            var newGroup = new Group { Name = "Test Group" };
            var expectedId = 42;

            // Simulation du comportement de la requête
            mockConnection
            .SetupDapperAsync(c => c.ExecuteScalarAsync<int>(It.IsAny<string>(), newGroup, null, null, null))
            .ReturnsAsync(expectedId);

            var repository = new GroupRepository(mockFactory.Object);

            // Lancement de la simulation
            var id = await repository.CreateGroup(newGroup);

            id.Should().Be(expectedId);
        }
    }
}