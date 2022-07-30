using Dapper;
using Library.Core.Interfaces.Factories;
using Library.Core.Interfaces.Repositories;
using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Repository;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(IConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        string query = @"SELECT id,
                                    title,
                                    author,
                                    publisher,
                                    bookcategoryid
                            FROM   book
                            WHERE  lenttostudentid IS NULL";

        using var connection = _connectionFactory.GetOpenConnection();

        return (await connection.QueryAsync<Book>(query)).AsList();
    }

    public async Task BorrowBookAsync(Guid id, string studentEmail)
    {
        string query = @"UPDATE book
                            SET lenttostudentid = (SELECT TOP 1 id
                                                    FROM   student
                                                    WHERE  email = @StudentEmail)
                            WHERE id = @BookId";

        using var connection = _connectionFactory.GetOpenConnection();

        await connection.ExecuteAsync(query, new
        {
            @StudentEmail = studentEmail,
            @BookId = id
        });
    }

    public async Task<bool> IsValidBookAsync(Guid id, string studentEmail)
    {
        string query = @"SELECT count(B.id)
                             FROM   [Library].[dbo].[book] B
                                    INNER JOIN [Library].[dbo].[coursebookscategories] CBC
                                           ON CBC.categoryid = B.bookcategoryid
                                    INNER JOIN [Library].[dbo].[student] S
                                           ON S.courseid = CBC.courseid
                             WHERE  email LIKE @StudentEmail
                                   AND B.id = @BookId";

        using var connection = _connectionFactory.GetOpenConnection();

        var validBook =  await connection.QueryFirstAsync<int>(query,
                        new
                        {
                            @StudentEmail = studentEmail,
                            @BookId = id
                        });

        return validBook > 0;
    }
}
