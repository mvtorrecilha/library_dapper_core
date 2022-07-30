using Library.Core.Commands;
using Library.Core.Common;
using Library.Core.Interfaces.Repositories;
using Library.Core.Notifications;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.Handlers;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand>
{
    private readonly IMediator _mediator;
    private readonly INotifier _notifier;
    private readonly IBookRepository _bookRepository;
    private readonly IStudentRepository _studentRepository;

    public BorrowBookCommandHandler(
        IMediator mediator,
        IBookRepository bookRepository,
        IStudentRepository studentRepository,
        INotifier notifier)
    {
        _mediator = mediator;
        _bookRepository = bookRepository;
        _notifier = notifier;
        _studentRepository = studentRepository;
    }

    public async Task<Unit> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        if (!await IsValidToBorrowABook(request))
        {
            return Unit.Value;
        }

        await _bookRepository.BorrowBookAsync(request.BookId, request.StudentEmail);
        await _mediator.Publish(new BorrowedBookNotification
        {
            BookId = request.BookId,
            StudentEmail = request.StudentEmail
        }, cancellationToken);

        return Unit.Value;
    }

    private async Task<bool> IsValidToBorrowABook(BorrowBookCommand request)
    {  
        if (String.IsNullOrWhiteSpace(request.StudentEmail))
        {
            _notifier.AddError("Email", Errors.EmailCannotBeEmpty, null);
            _notifier.SetStatuCode(HttpStatusCode.BadRequest);
            return false;
        }

        if (!await _studentRepository.IsStudentRegisteredByEmailAsync(request.StudentEmail))
        {
            _notifier.AddError("Email", Errors.StudentNotFound, request.StudentEmail);
            _notifier.SetStatuCode(HttpStatusCode.NotFound);
            return false;
        }

        if (!await _bookRepository.IsValidBookAsync(request.BookId, request.StudentEmail))
        {
            _notifier.AddError("Id", Errors.TheBookDoesNotBelongToTheCourseCategory, request.BookId);
            _notifier.SetStatuCode(HttpStatusCode.Forbidden);
            return false;
        }

        return true;
    }
}
