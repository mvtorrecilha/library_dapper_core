using Library.Core.Common;
using Library.Core.Interfaces.Services;
using Library.Core.Models.Requests;
using Moq;
using System;
using System.Threading.Tasks;

namespace Library.ApiTest.UnitTests.Mocks
{
    public class MockBookService : Mock<IBookService>
    {
        public MockBookService() : base(MockBehavior.Strict) { }

        public MockBookService MockBorrowBookAsyncWithCallBack2(Action<Notifier> callback)
        {
            Setup(m => m.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback(callback);

            return this;
        }

        public MockBookService MockBorrowBookAsyncWithCallBack(Action<BorrowRequest, string> callback)
        {
            Setup(m => m.BorrowBookAsync(It.IsAny<BorrowRequest>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback(callback);

            return this;
        }

        public MockBookService VerifyBorrowBookAsync(BorrowRequest borrowRequest, string action, Times times)
        {
            Verify(s => s.BorrowBookAsync(borrowRequest, action), times);

            return this;
        }
    }
}
