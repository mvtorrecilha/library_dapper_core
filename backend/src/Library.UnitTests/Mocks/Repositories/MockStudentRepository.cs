using Library.Core.Interfaces.Repositories;
using Moq;

namespace Library.UnitTests.Mocks.Repositories;

public class MockStudentRepository : Mock<IStudentRepository>
{
    public MockStudentRepository() : base(MockBehavior.Strict) { }

    public MockStudentRepository MockIsStudentRegisteredByEmailAsync(string studentEmail, bool output)
    {
        Setup(s => s.IsStudentRegisteredByEmailAsync(studentEmail)).ReturnsAsync(output);

        return this;
    }

    public MockStudentRepository VerifyIsStudentRegisteredByEmailAsync(string studentEmail, Times times)
    {
        Verify(s => s.IsStudentRegisteredByEmailAsync(studentEmail), times);

        return this;
    }
}
