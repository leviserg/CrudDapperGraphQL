using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooks(FilterModel filter);
        Task<Book> GetBook(int bookId);
        Task<Book> BookSave(Book book);
    }
}
