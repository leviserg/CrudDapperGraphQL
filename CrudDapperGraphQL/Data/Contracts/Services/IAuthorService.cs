using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthors(FilterModel filter);
        Task<Author> GetAuthor(int authorId);
        Task<Author> AuthorSave(Author author);
    }
}
