using Library.Core.Interfaces.Repositories;
using Library.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.UnitTests.Mocks.Repositories;

public class MockBookRepository : Mock<IBookRepository>
{
    public MockBookRepository() : base(MockBehavior.Strict) { }

    public MockBookRepository MockGetAllBooksAsync(List<Book> output)
    {
        Setup(m => m.GetAllBooksAsync())
            .ReturnsAsync(output);

        return this;
    }

    public MockBookRepository MockIsValidBookAsync(Guid id, string studentEmail, bool output)
    {
        Setup(s => s.IsValidBookAsync(id, studentEmail)).ReturnsAsync(output);

        return this;
    }

    public MockBookRepository MockBorrowBookAsync(Guid id, string studentEmail)
    {
        Setup(s => s.BorrowBookAsync(id, studentEmail)).Returns(Task.CompletedTask);

        return this;
    }

    public MockBookRepository VerifyIsValidBookAsync(Guid id, string studentEmail, Times times)
    {
        Verify(s => s.IsValidBookAsync(id, studentEmail), times);

        return this;
    }

    public MockBookRepository VerifyBorrowBookAsync(Guid id, string studentEmail, Times times)
    {
        Verify(s => s.BorrowBookAsync(id, studentEmail), times);

        return this;
    }
}
