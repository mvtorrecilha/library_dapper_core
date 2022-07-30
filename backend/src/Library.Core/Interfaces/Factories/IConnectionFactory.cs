using System.Data;

namespace Library.Core.Interfaces.Factories;

public interface IConnectionFactory
{
    IDbConnection GetOpenConnection();
}
