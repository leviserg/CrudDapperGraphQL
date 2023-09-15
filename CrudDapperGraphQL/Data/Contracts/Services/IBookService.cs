using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooks(FilterModel filter, CancellationToken cancellationToken = default);
        Task<Book> GetBook(int bookId, CancellationToken cancellationToken = default);
        Task<Book> BookSave(BookSave book);
        Task<bool> BookDelete(int bookId);
    }
}
