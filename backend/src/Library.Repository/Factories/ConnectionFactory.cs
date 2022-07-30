using Library.Core.Interfaces.Factories;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Library.Repository.Factories;

public class ConnectionFactory : IConnectionFactory
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionstring = "DefaultConnection";

    public ConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;  
    }

    public IDbConnection GetOpenConnection()
    {
        var connection = new SqlConnection(GetConnectionString());

        connection.Open();

        return connection;
    }

    private string GetConnectionString()
    {
        return _configuration.GetConnectionString(_connectionstring);
    }
}
