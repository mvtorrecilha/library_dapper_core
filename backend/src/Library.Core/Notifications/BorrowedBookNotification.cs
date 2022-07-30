using MediatR;
using System;

namespace Library.Core.Notifications;

public class BorrowedBookNotification : INotification
{
    public Guid BookId { get; set; }
    public string StudentEmail { get; set; }
}
