using Library.Core.Commands;
using Library.Core.Common;
using Library.Core.Handlers;
using Library.Core.Helpers;
using Library.Core.Interfaces.Factories;
using Library.Core.Interfaces.Repositories;
using Library.Repository;
using Library.Repository.Factories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Library.IoC;

public class NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        //Repositories
        services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
        services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
        services.AddScoped(typeof(IBookCategoryRepository), typeof(BookCategoryRepository));
        services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));

        //Services
        services.AddScoped(typeof(INotifier), typeof(Notifier));

        //Factories
        services.AddSingleton(typeof(IConnectionFactory), typeof(ConnectionFactory));

        //ResponseFormatter
        services.AddScoped(typeof(IResponseFormatter), typeof(ResponseFormatter));

        //Mediatr
        services.AddScoped<IRequestHandler<BorrowBookCommand, Unit>, BorrowBookCommandHandler>();
    }
}
