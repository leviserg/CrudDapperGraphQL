using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace CrudDapperGraphQL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Author>> GetAuthors(FilterModel filter)
        {
            return await _repository.GetAuthors(filter);
        }

        public async Task<Author> GetAuthor(int authorId)
        {
            var author = await _repository.GetAuthor(authorId);
            if (author == null)
            {
                throw new NotFoundException($"Author with ID {authorId} not found.");
            }
            return author;
        }

        public async Task<Author> AuthorSave(AuthorSave author)
        {
            try {
                return await _repository.AuthorSave(author);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<bool> AuthorDelete(int authorId)
        {
            return await _repository.AuthorDelete(authorId);
        }
    }
}
