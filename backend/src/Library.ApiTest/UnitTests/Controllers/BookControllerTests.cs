using FluentAssertions;
using Library.Api.Controllers;
using Library.Api.Helpers;
using Library.Api.ViewModels;
using Library.Core.Common;
using Library.Core.Interfaces.Services;
using Library.Core.Models;
using Library.Core.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace Library.ApiTest.UnitTests
{
    public class BookControllerTests
    {
        protected Mock<IBookService> _bookServiceMock;
        protected ResponseFormatter _responseFormatterMock;
        protected Notifier _notifier;
        private const string BorrowAction = "borrow";

        public BookControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _notifier = new Notifier();
        }

        private BookController GetController()
        {
            _responseFormatterMock = new ResponseFormatter(_notifier);
            return new BookController(_bookServiceMock.Object, _responseFormatterMock);
        }

        [Fact]
        public void PostBorrowBook_InvalidAction_ShouldBeNotFoundAndHasError()
        {
            // Arrange
            string invalidAction = "request";
            _bookServiceMock.Setup(x => x.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    _notifier.AddError("action", Errors.InvalidAction, It.IsAny<string>());
                    _notifier.SetStatuCode(HttpStatusCode.NotFound);
                });

            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid()
            };

            // Act
            var controller = GetController();
            var result = controller.Post(borrowRequest, invalidAction);

            // Assert
            _notifier.HasError.Should().BeTrue();
            _notifier.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(Errors.InvalidAction);
        }

        [Fact]
        public void PostBorrowBook_EmptyEmail_ShouldHasError()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()))
                .Callback(() => _notifier.AddError("Email", Errors.EmailCannotBeEmpty, null));

            var borrowRequest = new BorrowRequest
            {
                BookId = new Guid("762141A7-ACA3-4919-9ACD-3FC86815F05B")
            };

            // Act
            var controller = GetController();
            var result = controller.Post(borrowRequest, BorrowAction);

            // Assert
            string expectedError = Errors.EmailCannotBeEmpty;
            _notifier.HasError.Should().BeTrue();
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(expectedError);
        }

        [Fact]
        public void PostBorrowBook_StudentNotFoundByEmailDataBase_ShouldHasError()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()))
                .Callback(() => _notifier.AddError("Email", Errors.StudentNotFound, It.IsAny<string>()));

            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid(),
                StudentEmail = "teste@gmail.com"
            };

            // Act
            var controller = GetController();
            var result = controller.Post(borrowRequest, BorrowAction);

            // Assert
            string expectedError = Errors.StudentNotFound;
            _notifier.HasError.Should().BeTrue();
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(expectedError);
        }

        [Fact]
        public void PostBorrowBook_BookDoesntBelongToStudentCourse_ShouldHasError()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()))
                .Callback(() => _notifier.AddError("Id",
                                                    Errors.TheBookDoesNotBelongToTheCourseCategory,
                                                    It.IsAny<Guid>()));

            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid(),
                StudentEmail = "hdinizribeiro@gmail.com"
            };

            // Act
            var controller = GetController();
            var result = controller.Post(borrowRequest, BorrowAction);

            // Assert
            string expectedError = Errors.TheBookDoesNotBelongToTheCourseCategory;
            _notifier.HasError.Should().BeTrue();
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(expectedError);
        }

        [Fact]
        public void PostBorrowBook_ValidBookAndEmail_ShouldBeSuccess()
        {
            // Arrange
            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid(),
                StudentEmail = "hdinizribeiro@gmail.com"
            };

            // Act
            var controller = GetController();
            var result = controller.Post(borrowRequest, BorrowAction);

            // Assert
            _notifier.HasError.Should().BeFalse();
            _bookServiceMock.Verify(x => x.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()), Times.Once);
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

            var expected = new OkObjectResult(bookList.Select(b => new BookViewModel()
            {
                Author = b.Author,
                Id = b.Id,
                Pages = b.Pages,
                Publisher = b.Publisher,
                Title = b.Title
            }));

            _bookServiceMock.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(bookList);

            // Act
            var controller = GetController();
            var result = controller.Get().Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

    }
}
