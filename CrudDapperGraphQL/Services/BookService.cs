using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using SendGrid.Helpers.Errors.Model;

namespace CrudDapperGraphQL.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book, BookSave> _repository;
        public BookService(IRepository<Book, BookSave> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Book>> GetBooks(FilterModel filter)
        {
            return await _repository.GetAll(filter);
        }

        public async Task<Book> GetBook(int bookId)
        {
            var book = await _repository.GetById(bookId);
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
                return await _repository.Save(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> BookDelete(int bookId)
        {
            return await _repository.Delete(bookId);
        }
    }
}
