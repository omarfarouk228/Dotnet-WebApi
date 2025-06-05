using System.Data;

namespace ConsoleApp1.Data.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> GetOpenConnectionInterface();
    }
}