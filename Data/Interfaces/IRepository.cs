using Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> FindAll();

        Task<TEntity> GetByIdAsync(int id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteByIdAsync(int id);
    }
}