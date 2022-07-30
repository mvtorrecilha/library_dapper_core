using Library.Core.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Core.EventHandlers;

public class LogEventHandler : INotificationHandler<BorrowedBookNotification>
{
    public Task Handle(BorrowedBookNotification notification, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            Console.WriteLine($"Borrowed book: '{notification.BookId} - {notification.StudentEmail}'");
        });
    }
}
