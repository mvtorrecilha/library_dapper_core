using Library.Core.Interfaces.Factories;
using Library.Core.Interfaces.Repositories;
using Library.Core.Models;

namespace Library.Repository;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(IConnectionFactory connectionFactory)
       : base(connectionFactory) { }
}
