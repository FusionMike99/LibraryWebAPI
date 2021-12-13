using Data.Interfaces;
using Data.Repositories;
using System.Threading.Tasks;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext context;
        private BookRepository bookRepository;
        private CardRepository cardRepository;
        private HistoryRepository historyRepository;
        private ReaderRepository readerRepository;

        public UnitOfWork(LibraryDbContext context)
        {
            this.context = context;
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepository(context);
                return bookRepository;
            }
        }

        public ICardRepository CardRepository
        {
            get
            {
                if (cardRepository == null)
                    cardRepository = new CardRepository(context);
                return cardRepository;
            }
        }

        public IHistoryRepository HistoryRepository
        {
            get
            {
                if (historyRepository == null)
                    historyRepository = new HistoryRepository(context);
                return historyRepository;
            }
        }

        public IReaderRepository ReaderRepository
        {
            get
            {
                if (readerRepository == null)
                    readerRepository = new ReaderRepository(context);
                return readerRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            var result = await context.SaveChangesAsync();
            return result;
        }
    }
}