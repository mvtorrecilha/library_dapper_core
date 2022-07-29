using FluentAssertions;
using Library.Api.Controllers;
using Library.Api.Helpers;
using Library.Api.ViewModels;
using Library.ApiTest.UnitTests.Mocks;
using Library.ApiTest.UnitTests.Mocks.Repositories;
using Library.Core.Commands;
using Library.Core.Common;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Library.ApiTest.UnitTests
{
    public class BookControllerTests
    {
        protected MockBookRepository _mockBookRepository;
        protected ResponseFormatter _responseFormatterMock;
        protected Notifier _notifier;
        protected MockMediatr _mockMediatr;

        public BookControllerTests()
        {
            _mockBookRepository = new MockBookRepository();
            _notifier = new Notifier();
            _mockMediatr = new MockMediatr();
        }

        private BookController GetController()
        {
            _responseFormatterMock = new ResponseFormatter(_notifier);
            return new BookController(
                _mockBookRepository.Object, _responseFormatterMock, _mockMediatr.Object);
        }

        [Fact]
        public async Task PostBorrowBook_BorrowedBook_Successfully()
        {
            // Arrange
            _mockMediatr
                .MockSendCommand<BorrowBookCommand>();

            var request = new BorrowBookCommand
            {
                BookId = new Guid("762141A7-ACA3-4919-9ACD-3FC86815F05B")
            };

            // Act
            var controller = GetController();
            await controller.Post(request);

            // Assert
            _mockMediatr.VerifySend<BorrowBookCommand>(Times.Once());
        }

        [Fact]
        public void PostBorrowBook_BookNotFound_ThrowException()
        {
            // Arrange
            Exception exception = new Exception("Book Not found");
            _mockMediatr
                .MockSendWithException<BorrowBookCommand>(exception);

            var request = new BorrowBookCommand
            {
                BookId = Guid.NewGuid()
            };

            // Act
            var controller = GetController();
            Func<Task> action = async () => await controller.Post(request);

            // Assert
            var exception2 = action.Should().ThrowAsync<Exception>();
            exception2.WithMessage("Book Not found");    
        }

        [Fact]
        public void GetBooks_ReturnAllBooks()
        {
            //Arrange
            var bookList = new List<Book>
            {
                new Book
                {
                    Id = new Guid(),
                    Author = "Author1",
                    Pages = 0,
                    Publisher = "",
                    Title = "Title1",
                    BookCategoryId = new Guid()
                },
                new Book
                {
                    Id = new Guid(),
                    Author = "Author2",
                    Pages = 0,
                    Publisher = "",
                    Title = "Title2",
                    BookCategoryId = new Guid()
                }
            };

            _mockBookRepository.MockGetAllBooksAsync(bookList);              

            // Act
            var controller = GetController();
            var result = controller.Get().Result;

            // Assert
            var expected = new OkObjectResult(bookList.Select(b => new BookViewModel()
            {
                Author = b.Author,
                Id = b.Id,
                Pages = b.Pages,
                Publisher = b.Publisher,
                Title = b.Title
            }));

            result.Should().BeEquivalentTo(expected);
        }
    }
}
