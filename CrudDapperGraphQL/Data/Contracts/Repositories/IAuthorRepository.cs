using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthors(FilterModel filter);
        Task<Author> GetAuthor(int authorId);
        Task<Author> AuthorSave(AuthorSave author);
        Task<bool> AuthorDelete(int authorId);

    }
}
