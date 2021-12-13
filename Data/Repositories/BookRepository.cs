using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext context;

        public BookRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Book entity)
        {
            await context.Books.AddAsync(entity);
        }

        public void Delete(Book entity)
        {
            context.Books.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var element = await GetByIdAsync(id);
            context.Books.Remove(element);
        }

        public IQueryable<Book> FindAll()
        {
            var elements = context.Books.AsQueryable();
            return elements;
        }

        public IQueryable<Book> FindAllWithDetails()
        {
            var elements = context.Books
                .Include(b => b.Cards)
                .AsQueryable();
            return elements;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var element = await context.Books.FindAsync(id);
            return element;
        }

        public async Task<Book> GetByIdWithDetailsAsync(int id)
        {
            var element = await context.Books
                .Include(b => b.Cards)
                .SingleOrDefaultAsync(b => b.Id == id);
            return element;
        }

        public void Update(Book entity)
        {
            context.Books.Update(entity);
        }
    }
}
