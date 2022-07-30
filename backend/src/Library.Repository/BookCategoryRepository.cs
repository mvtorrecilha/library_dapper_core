using Library.Core.Interfaces.Factories;
using Library.Core.Interfaces.Repositories;
using Library.Core.Models;

namespace Library.Repository;

public class BookCategoryRepository : BaseRepository<BookCategory>, IBookCategoryRepository
{
    public BookCategoryRepository(IConnectionFactory connectionFactory)
        : base(connectionFactory) { }
}
