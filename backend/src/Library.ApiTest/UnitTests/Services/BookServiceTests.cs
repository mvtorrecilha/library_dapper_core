using FluentAssertions;
using Library.ApiTest.UnitTests.Mocks;
using Library.ApiTest.UnitTests.Mocks.Repositories;
using Library.Core.Common;
using Library.Core.Models.Requests;
using Library.Service;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Library.ApiTest.UnitTests.Services
{
    public class BookServiceTests
    {
        protected MockBookService _bookServiceMock;
        protected MockBookRepository _bookRepositoryMock;
        protected MockStudentRepository _studentRepositoryMock;
        protected Notifier _notifier;
        private readonly BookService _bookingService;

        public BookServiceTests()
        {
            _bookServiceMock = new MockBookService();
            _bookRepositoryMock = new MockBookRepository();
            _studentRepositoryMock = new MockStudentRepository();
            _notifier = new Notifier();

            _bookingService = new BookService(
                _bookRepositoryMock.Object,
                _notifier,
                _studentRepositoryMock.Object);
        }

        [Fact]
        public async Task BorrowBookAsync_CallingWithWrongAction_ReturnError()
        {
            // Arrange
            string action = "request";
            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid()
            };

            // Act
            await _bookingService.BorrowBookAsync(borrowRequest, action);

            // Assert
            _notifier.HasError.Should().BeTrue();
            _notifier.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(Errors.InvalidAction);
            _bookServiceMock.VerifyBorrowBookAsync(borrowRequest, action, Times.Never());
        }

        [Fact]
        public async Task BorrowBookAsync_CallingWithEmptyStudentEmail_ReturnError()
        {
            // Arrange
            string action = "borrow";
            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid()
            };

            // Act
            await _bookingService.BorrowBookAsync(borrowRequest, action);

            // Assert
            _notifier.HasError.Should().BeTrue();
            _notifier.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(Errors.EmailCannotBeEmpty);
            _bookServiceMock.VerifyBorrowBookAsync(borrowRequest, action, Times.Never());
        }

        [Fact]
        public async Task BorrowBookAsync_StudentNotRegistered_ReturnError()
        {
            // Arrange
            string action = "borrow";
            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid(),
                StudentEmail = "teste@test.com"

            };

            _studentRepositoryMock.MockIsStudentRegisteredByEmailAsync(borrowRequest.StudentEmail, false);

            // Act
            await _bookingService.BorrowBookAsync(borrowRequest, action);

            // Assert
            _notifier.HasError.Should().BeTrue();
            _notifier.StatusCode.Should().Be(HttpStatusCode.NotFound);
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(Errors.StudentNotFound);
            _bookServiceMock.VerifyBorrowBookAsync(borrowRequest, action, Times.Never());
        }

        [Fact]
        public async Task BorrowBookAsync_TheBookDoesNotBelongToTheCourseCategory_ReturnError()
        {
            // Arrange
            string action = "borrow";
            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid(),
                StudentEmail = "teste@test.com"

            };

            _studentRepositoryMock.MockIsStudentRegisteredByEmailAsync(borrowRequest.StudentEmail, true);
            _bookRepositoryMock.MockIsValidBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail, false);

            // Act
            await _bookingService.BorrowBookAsync(borrowRequest, action);

            // Assert
            _notifier.HasError.Should().BeTrue();
            _notifier.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            _notifier.Errors.Should().ContainSingle()
                .Which.Message.Should().Be(Errors.TheBookDoesNotBelongToTheCourseCategory);
            _bookServiceMock.VerifyBorrowBookAsync(borrowRequest, action, Times.Never());
        }

        [Fact]
        public async Task BorrowBookAsync_IsValidToBook_BorrowABook()
        {
            // Arrange
            string action = "borrow";
            var borrowRequest = new BorrowRequest
            {
                BookId = Guid.NewGuid(),
                StudentEmail = "teste@test.com"

            };

            _studentRepositoryMock.MockIsStudentRegisteredByEmailAsync(borrowRequest.StudentEmail, true);
            _bookRepositoryMock.MockIsValidBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail, true);
            _bookRepositoryMock.MockBorrowBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail);

            // Act
            await _bookingService.BorrowBookAsync(borrowRequest, action);

            // Assert
            _notifier.HasError.Should().BeFalse();
            _studentRepositoryMock.VerifyIsStudentRegisteredByEmailAsync(borrowRequest.StudentEmail, Times.Once());
            _bookRepositoryMock.VerifyIsValidBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail, Times.Once());
            _bookRepositoryMock.VerifyBorrowBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail, Times.Once());
        }
    }
}
