using Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IReaderRepository : IRepository<Reader>
    {
        IQueryable<Reader> GetAllWithDetails();
        Task<Reader> GetByIdWithDetails(int id);
    }
}
