using System;

namespace Library.Core.Models.Requests
{
    public class BorrowRequest
    {
        public Guid BookId { get; set; }
        public string StudentEmail { get; set; }
    }
}
