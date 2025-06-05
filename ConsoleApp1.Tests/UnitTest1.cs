using System.Data;
using ConsoleApp1.Data;
using FluentAssertions;

namespace ConsoleApp1.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var connectionString = "Server=localhost;Database=MediaDB;User Id=SA;Password=Omar1999;TrustServerCertificate=True;";
        var factory = new SqlConnectionFactory(connectionString);

        using var connection = await factory.GetOpenConnectionInterface();

        // Check with Assert
        connection.Should().NotBeNull();
        connection.State.Should().Be(ConnectionState.Open);
    }

    [Fact]
    public async Task Test2Async()
    {
        Assert.Equal(1, 1);
    }
}
