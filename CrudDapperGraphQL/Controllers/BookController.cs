using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace CrudDapperGraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IEntityService<Book, BookSave> _service;
        public BookController(IEntityService<Book, BookSave> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<IActionResult> GetBooks([FromQuery] FilterModel filter)
        {
            try
            {
                filter = filter ?? new FilterModel();
                var books = await _service.GetAll(filter);
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
        [Authorize]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _service.GetById(id);
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
        [Authorize]
        public async Task<IActionResult> BookSave([FromBody] BookSave book)
        {
            try
            {
                var savedBook = await _service.Save(book);
                return Ok(savedBook);
            }
            catch (Exception ex)
            {
                // Log or handle unexpected errors
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> BookDelete(int id)
        {
            var result = await _service.Delete(id);
            return (result) ? Ok(true) : StatusCode(409, "Conflict");
        }

        [HttpGet]
        [Route("error/{id:int}")]
        public async Task<IActionResult> GetException(int id)
        {
            switch (id)
            {
                case 500: throw new ApplicationException("Common server error");
                case 401: throw new ForbiddenException("Requested resource forbidden");
                case 403: throw new UnauthorizedAccessException("You're not authorized to get the requested resource");
                case 404: throw new NotFoundException("Requiested object not found");
                default:
                    break;
            }
            return Ok(true);
    }
}
}
