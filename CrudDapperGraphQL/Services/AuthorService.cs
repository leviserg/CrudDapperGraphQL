using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using SendGrid.Helpers.Errors.Model;

namespace CrudDapperGraphQL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author, AuthorSave> _repository;
        public AuthorService(IRepository<Author, AuthorSave> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Author>> GetAuthors(FilterModel filter)
        {
            return await _repository.GetAll(filter);
        }

        public async Task<Author> GetAuthor(int authorId)
        {
            var author = await _repository.GetById(authorId);
            if (author == null)
            {
                throw new NotFoundException($"Author with ID {authorId} not found.");
            }
            return author;
        }

        public async Task<Author> AuthorSave(AuthorSave author)
        {
            try {
                return await _repository.Save(author);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> AuthorDelete(int authorId)
        {
            return await _repository.Delete(authorId);
        }
    }
}
