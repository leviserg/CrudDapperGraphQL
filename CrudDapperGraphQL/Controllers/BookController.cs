using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using CrudDapperGraphQL.Services;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;

namespace CrudDapperGraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetBooks([FromQuery] FilterModel filter)
        {
            try
            {
                filter = filter ?? new FilterModel();
                var books = await _bookService.GetBooks(filter);
                return Ok(books);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBook(id);
                return Ok(book);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log or handle unexpected errors
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> BookSave([FromBody] Book book)
        {
            try
            {
                var savedBook = await _bookService.BookSave(book);
                return Ok(savedBook);
            }
            catch (Exception ex)
            {
                // Log or handle unexpected errors
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
