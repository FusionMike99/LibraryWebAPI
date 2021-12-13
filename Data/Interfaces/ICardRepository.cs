using Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card> GetByIdWithDetailsAsync(int id);
        IQueryable<Card> FindAllWithDetails();
    }
}
