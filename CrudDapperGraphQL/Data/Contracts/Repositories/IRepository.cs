using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.Contracts.Repositories
{
    public interface IRepository<T, TSave>
    {
        Task<IEnumerable<T>> GetAll(FilterModel filter);
        Task<T> GetById(int id);
        Task<T> Save(TSave saveModel);
        Task<bool> Delete(int id);
    }
}
