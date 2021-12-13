using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly LibraryDbContext context;

        public CardRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Card entity)
        {
            await context.Cards.AddAsync(entity);
        }

        public void Delete(Card entity)
        {
            context.Cards.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var element = await GetByIdAsync(id);
            context.Cards.Remove(element);
        }

        public IQueryable<Card> FindAll()
        {
            var elements = context.Cards.AsQueryable();
            return elements;
        }

        public IQueryable<Card> FindAllWithDetails()
        {
            var elements = context.Cards
                .Include(c => c.Reader)
                .Include(c => c.Books)
                .AsQueryable();
            return elements;
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            var element = await context.Cards.FindAsync(id);
            return element;
        }

        public async Task<Card> GetByIdWithDetailsAsync(int id)
        {
            var element = await context.Cards
                .Include(c => c.Reader)
                .Include(c => c.Books)
                .SingleOrDefaultAsync(c => c.Id == id);
            return element;
        }

        public void Update(Card entity)
        {
            context.Cards.Update(entity);
        }
    }
}
