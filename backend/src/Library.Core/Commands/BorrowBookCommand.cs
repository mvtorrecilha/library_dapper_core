using MediatR;
using System;

namespace Library.Core.Commands;

public class BorrowBookCommand : IRequest
{
    public Guid BookId { get; set; }
    public string StudentEmail { get; set; }
}
