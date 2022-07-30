using Library.Core.Commands;
using Library.Core.Helpers;
using Library.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IResponseFormatter _responseFormatter;
    private readonly IMediator _mediator;

    public BookController(IBookRepository bookRepository,
                          IResponseFormatter responseFormatter,
                          IMediator mediator)
    {
        _bookRepository = bookRepository;
        _responseFormatter = responseFormatter;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("/api/books")]
    public async Task<IActionResult> Get()
    {
        var books = await _bookRepository.GetAllBooksAsync();

        return Ok(books);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status404NotFound)]   
    [Route("/api/books/{bookId:guid}/student/{studentEmail}/borrow")]
    public async Task<IActionResult> Post([FromRoute] BorrowBookCommand command)
    {
        await _mediator.Send(command);
        return _responseFormatter.Format();
    }
}
