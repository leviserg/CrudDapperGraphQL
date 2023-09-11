using CrudDapperGraphQL.Data.Contracts;
using CrudDapperGraphQL.Data.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
