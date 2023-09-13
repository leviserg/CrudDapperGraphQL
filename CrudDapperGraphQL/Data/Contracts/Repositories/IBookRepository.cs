using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooks(FilterModel filter);
        Task<Book> GetBook(int bookId);
        Task<Book> BookSave(BookSave book);
    }
}
