using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetBooks(FilterModel filter);
    }
}
