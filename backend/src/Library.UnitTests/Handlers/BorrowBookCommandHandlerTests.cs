using FluentAssertions;
using Library.UnitTests.Mocks;
using Library.UnitTests.Mocks.Repositories;
using Library.Core.Commands;
using Library.Core.Common;
using Library.Core.Handlers;
using Library.Core.Helpers;
using Library.Core.Notifications;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Library.UnitTests.Handlers;

public class BorrowBookCommandHandlerTests
{
    protected MockBookRepository _mockBookRepository;
    protected MockStudentRepository _mockStudentRepository;
    protected ResponseFormatter _responseFormatterMock;
    protected Notifier _notifier;
    protected MockMediatr _mockMediatr;
    private readonly BorrowBookCommandHandler _handler;

    public BorrowBookCommandHandlerTests()
    {
        _mockBookRepository = new MockBookRepository();
        _mockStudentRepository = new MockStudentRepository();
        _notifier = new Notifier();
        _mockMediatr = new MockMediatr();

        _handler = new BorrowBookCommandHandler(
            _mockMediatr.Object,
            _mockBookRepository.Object,
            _mockStudentRepository.Object,
            _notifier);
    }

    [Fact]
    public async Task HandleBorrowBookCommand_EmptyEmail_ShouldHasError()
    {
        // Arrange
        var command = new BorrowBookCommand
        {
            BookId = new Guid("762141A7-ACA3-4919-9ACD-3FC86815F05B")
        };

        //Act
        await _handler.Handle(command, default);

        // Assert
        string expectedError = Errors.EmailCannotBeEmpty;
        _notifier.HasError.Should().BeTrue();
        _notifier.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(expectedError);
    }

    [Fact]
    public async Task HandleBorrowBookCommand_StudentNotFoundByEmail_ShouldHasError()
    {
        // Arrange
        var command = new BorrowBookCommand
        {
            BookId = new Guid("762141A7-ACA3-4919-9ACD-3FC86815F05B"),
            StudentEmail = "test@gmail.com"
        };

        _mockStudentRepository.MockIsStudentRegisteredByEmailAsync(command.StudentEmail, false);

        //Act
        await _handler.Handle(command, default);

        // Assert
        string expectedError = Errors.StudentNotFound;
        _notifier.HasError.Should().BeTrue();
        _notifier.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(expectedError);
    }

    [Fact]
    public async Task HandleBorrowBookCommand_BookDoesntBelongToStudentCourse_ShouldHasError()
    {
        // Arrange
        var command = new BorrowBookCommand
        {
            BookId = new Guid("762141A7-ACA3-4919-9ACD-3FC86815F05B"),
            StudentEmail = "jr@gmail.com"
        };

        _mockStudentRepository.MockIsStudentRegisteredByEmailAsync(command.StudentEmail, true);
        _mockBookRepository.MockIsValidBookAsync(command.BookId, command.StudentEmail, false);

        //Act
        await _handler.Handle(command, default);

        // Assert
        string expectedError = Errors.TheBookDoesNotBelongToTheCourseCategory;
        _notifier.HasError.Should().BeTrue();
        _notifier.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(expectedError);
    }

    [Fact]
    public async Task HandleBorrowBookCommand_ValidBookAndEmail_ShouldBeSuccess()
    {
        // Arrange
        var command = new BorrowBookCommand
        {
            BookId = new Guid("762141A7-ACA3-4919-9ACD-3FC86815F05B"),
            StudentEmail = "jr@gmail.com"
        };

        _mockStudentRepository.MockIsStudentRegisteredByEmailAsync(command.StudentEmail, true);
        _mockBookRepository.MockIsValidBookAsync(command.BookId, command.StudentEmail, true);
        _mockBookRepository.MockBorrowBookAsync(command.BookId, command.StudentEmail);
        _mockMediatr.MockPublish<BorrowedBookNotification>();

        //Act
        await _handler.Handle(command, default);

        // Assert
        _notifier.HasError.Should().BeFalse();
        _mockBookRepository.VerifyBorrowBookAsync(command.BookId, command.StudentEmail, Times.Once());
        _mockMediatr.VerifyPublish<BorrowedBookNotification>(Times.Once());
    }
}
