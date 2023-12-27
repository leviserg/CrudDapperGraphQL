using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace CrudDapperGraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IEntityService<Author, AuthorSave> _service;
        public AuthorController(IEntityService<Author, AuthorSave> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<IActionResult> GetAuthors([FromQuery] FilterModel filter)
        {
            try
            {
                filter = filter ?? new FilterModel();
                var authors = await _service.GetAll(filter);
                return Ok(authors);
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
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                var author = await _service.GetById(id);
                return Ok(author);
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
        public async Task<IActionResult> AuthorSave([FromBody] AuthorSave author)
        {
            try
            {
                var savedAuthor = await _service.Save(author);
                return Ok(savedAuthor);
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
        public async Task<IActionResult> AuthorDelete(int id)
        {
            var result = await _service.Delete(id);
            return (result) ? Ok(true) : StatusCode(409, "Conflict");
        }

    }
}
