using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts
{
    public interface IBookAuthorRepository
    {
        public Task<IEnumerable<Book>> GetBooks(FilterModel filter);
    }
}
