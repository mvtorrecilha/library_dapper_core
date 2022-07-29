﻿using Library.Api.Helpers;
using Library.Api.ViewModels;
using Library.Core.Commands;
using Library.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Controllers
{
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
       // [Authorize]
        [Route("/api/books")]
        public async Task<IActionResult> Get()
        {
            var books = await _bookRepository.GetAllBooksAsync();

            return Ok(books.Select(b => new BookViewModel()
            {
                Author = b.Author,
                Id = b.Id,
                Pages = b.Pages,
                Publisher = b.Publisher,
                Title = b.Title
            }));
        }

        [HttpPost]
        //[Authorize]
        [Route("/api/books/{bookId:guid}/student/{studentEmail}")]
        public async Task<IActionResult> Post([FromRoute] BorrowBookCommand command)
        {
            await _mediator.Send(command);
            return _responseFormatter.Format();
        }
    }
}