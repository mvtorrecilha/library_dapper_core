using Library.Core.Common;
using Library.Core.Interfaces.Repositories;
using Library.Core.Models.Requests;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Library.Service
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly INotifier _notifier;
        private readonly IStudentRepository _studentRepository;
        private const string BorrowAction = "borrow";

        public BookService(IBookRepository bookRepository,
                           INotifier notifier,
                           IStudentRepository studentRepository)
        {
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _notifier = notifier;
        }

        /// <inheritdoc/>
        public async Task BorrowBookAsync(BorrowRequest borrowRequest, string action) 
        {
            if (!await IsValidToBorrowABook(borrowRequest, action))
            {
                return;
            }

            await _bookRepository.BorrowBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail);                
        }

        private async Task<bool> IsValidToBorrowABook(BorrowRequest borrowRequest, string action)
        {
            if (!action.ToLower().Equals(BorrowAction))
            {
                _notifier.AddError("action", Errors.InvalidAction, action);
                _notifier.SetStatuCode(HttpStatusCode.NotFound);
                return false;
            }

            if (String.IsNullOrWhiteSpace(borrowRequest.StudentEmail))
            {
                _notifier.AddError("Email", Errors.EmailCannotBeEmpty, null);
                _notifier.SetStatuCode(HttpStatusCode.BadRequest);
                return false;
            }

            if (!await _studentRepository.IsStudentRegisteredByEmailAsync(borrowRequest.StudentEmail))
            {
                _notifier.AddError("Email", Errors.StudentNotFound, borrowRequest.StudentEmail);
                _notifier.SetStatuCode(HttpStatusCode.NotFound);
                return false;
            }

            if (!await _bookRepository.IsValidBookAsync(borrowRequest.BookId, borrowRequest.StudentEmail))
            {
                _notifier.AddError("Id", Errors.TheBookDoesNotBelongToTheCourseCategory, borrowRequest.BookId);
                _notifier.SetStatuCode(HttpStatusCode.Forbidden);
                return false;
            }

            return true;
        }
    }
}
