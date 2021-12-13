using Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IQueryable<Book> FindAllWithDetails();

        Task<Book> GetByIdWithDetailsAsync(int id);
    }
}