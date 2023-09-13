using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using SendGrid.Helpers.Errors.Model;
using static Dapper.SqlMapper;

namespace CrudDapperGraphQL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Book>> GetBooks(FilterModel filter)
        {
            return await _repository.GetBooks(filter);
        }

        public async Task<Book> GetBook(int bookId)
        {
            var book = await _repository.GetBook(bookId);
            if (book == null)
            {
                throw new NotFoundException($"Book with ID {bookId} not found.");
            }
            return book;
        }

        public async Task<Book> BookSave(BookSave book)
        {
            try
            {
                return await _repository.BookSave(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
