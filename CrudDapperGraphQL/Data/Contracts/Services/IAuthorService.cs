using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthors(FilterModel filter, CancellationToken cancellationToken = default);
        Task<Author> GetAuthor(int authorId, CancellationToken cancellationToken = default);
        Task<Author> AuthorSave(AuthorSave author);
        Task<bool> AuthorDelete(int authorId);
    }
}
