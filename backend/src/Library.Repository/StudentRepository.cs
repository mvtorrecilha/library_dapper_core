using Dapper;
using Library.Core.Interfaces.Factories;
using Library.Core.Interfaces.Repositories;
using Library.Core.Models;
using System.Threading.Tasks;

namespace Library.Repository;

public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
    public StudentRepository(IConnectionFactory connectionFactory)
       : base(connectionFactory) { }

    public async Task<bool> IsStudentRegisteredByEmailAsync(string studentEmail)
    {
        string query = @"SELECT Count (id)
                            FROM   [Library].[dbo].[student]
                            WHERE  email LIKE @StudentEmail ";

        using var connection = _connectionFactory.GetOpenConnection();

        var validStudent = await connection.QueryFirstAsync<int>(query,
                        new
                        {
                            @StudentEmail = studentEmail
                        });

        return validStudent > 0;
    }
}
