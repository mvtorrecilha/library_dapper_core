using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.UnitTests.Mocks;

public class MockMediatr : Mock<IMediator>
{
    public MockMediatr() : base(MockBehavior.Strict) { }

    public MockMediatr MockSendCommand<TCommand>() where TCommand : IRequest
    {
        Setup(s => s.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        return this;
    }

    public MockMediatr MockSendWithException<TCommand>(Exception exception) where TCommand : IRequest
    {
        Setup(s => s.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
            .Throws(exception);

        return this;
    }

    public MockMediatr MockPublish<T>() where T : INotification
    {
        Setup(s => s.Publish(It.IsAny<T>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return this;
    }

    public MockMediatr VerifySend<TCommand>(Times times) where TCommand : IRequest
    {
        Verify(s => s.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()), times);

        return this;
    }

    public MockMediatr VerifyPublish<T>(Times times) where T : INotification
    {
        Verify(s => s.Publish(It.IsAny<T>(), It.IsAny<CancellationToken>()), times);

        return this;
    }
}
