using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly LibraryDbContext context;

        public HistoryRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(History entity)
        {
            await context.Histories.AddAsync(entity);
        }

        public void Delete(History entity)
        {
            context.Histories.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var element = await GetByIdAsync(id);
            context.Histories.Remove(element);
        }

        public IQueryable<History> FindAll()
        {
            var elements = context.Histories.AsQueryable();
            return elements;
        }

        public IQueryable<History> GetAllWithDetails()
        {
            var elements = context.Histories
                .Include(h => h.Book)
                .Include(h => h.Card)
                .ThenInclude(c => c.Reader)
                .ThenInclude(c => c.ReaderProfile)
                .AsQueryable();
            return elements;
        }

        public async Task<History> GetByIdAsync(int id)
        {
            var element = await context.Histories.FindAsync(id);
            return element;
        }

        public void Update(History entity)
        {
            context.Histories.Update(entity);
        }
    }
}
