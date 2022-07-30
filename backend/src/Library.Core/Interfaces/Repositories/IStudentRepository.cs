using Library.Core.Models;
using System.Threading.Tasks;

namespace Library.Core.Interfaces.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<bool> IsStudentRegisteredByEmailAsync(string studentEmail);
}
