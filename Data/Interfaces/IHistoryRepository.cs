using Data.Entities;
using System.Linq;

namespace Data.Interfaces
{
    public interface IHistoryRepository : IRepository<History>
    {
        IQueryable<History> GetAllWithDetails();
    }
}