using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryDbContext context;

        public ReaderRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Reader entity)
        {
            await context.Readers.AddAsync(entity);
        }

        public void Delete(Reader entity)
        {
            context.Readers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var element = await GetByIdAsync(id);
            context.Readers.Remove(element);
        }

        public IQueryable<Reader> FindAll()
        {
            var elements = context.Readers.AsQueryable();
            return elements;
        }

        public IQueryable<Reader> GetAllWithDetails()
        {
            var elements = context.Readers
                .Include(r => r.ReaderProfile)
                .Include(r => r.Cards)
                .AsQueryable();
            return elements;
        }

        public async Task<Reader> GetByIdAsync(int id)
        {
            var element = await context.Readers.FindAsync(id);
            return element;
        }

        public async Task<Reader> GetByIdWithDetails(int id)
        {
            var element = await context.Readers
                .Include(r => r.ReaderProfile)
                .Include(r => r.Cards)
                .SingleOrDefaultAsync(r => r.Id == id);
            return element;
        }

        public void Update(Reader entity)
        {
            context.Readers.Update(entity);
        }
    }
}
