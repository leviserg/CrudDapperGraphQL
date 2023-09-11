using CrudDapperGraphQL.Data.Contracts;
using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookAuthorRepository _repository;
        public BookService(IBookAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Book>> GetBooks(FilterModel filter)
        {
            return await _repository.GetBooks(filter);
        }
    }
}
