using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }

        ICardRepository CardRepository { get; }

        IHistoryRepository HistoryRepository { get; }

        IReaderRepository ReaderRepository { get; }

        Task<int> SaveAsync();
    }
}