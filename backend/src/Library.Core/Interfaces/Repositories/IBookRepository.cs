using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Core.Interfaces.Repositories;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task BorrowBookAsync(Guid id, string studentEmail);
    Task<bool> IsValidBookAsync(Guid id, string studentEmail);
}
