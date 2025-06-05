using System.Text.RegularExpressions;
using ConsoleApp1.Data;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Data.Repositories;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

// Créer une instance de la classe DataAccessBase
// var dataAccess = new DataAccessBase(connectionString!);

// Tester la connexion
// bool canConnect = await dataAccess.CanConnect();

IDbConnectionFactory connectionFactory = new SqlConnectionFactory(connectionString!);

using var testConnection = await connectionFactory.GetOpenConnectionInterface();

var canConnect = testConnection.State == System.Data.ConnectionState.Open;

if (canConnect)
{
    IGroupRepository groupRepository = new GroupRepository(connectionFactory);
    IMediaRepository mediaRepository = new MediaRepository(connectionFactory);

    var group = new ConsoleApp1.Models.Group { Name = "Group 2" };
    var groupId = await groupRepository.CreateGroup(group);
    Console.WriteLine($"Nouveau groupe avec l'ID : {groupId}");

    var media = new ConsoleApp1.Models.Media { Name = "Media 2", GroupID = groupId, Type = "Pdf", Status = 0 };
    var mediaId = await mediaRepository.CreateMediaAsync(media);
    Console.WriteLine($"Nouveau media avec l'ID : {mediaId}");

    var groupWithMedias = await groupRepository.GetGroupWithMedias(groupId);
    Console.WriteLine($"Groupe : {groupWithMedias!.Name}");
    foreach (var mediaItem in groupWithMedias.Medias)
    {
        Console.WriteLine($"- {mediaItem.Name} ({mediaItem.Type})");
    }
}
else
{
    Console.WriteLine("Connexion impossible");
}