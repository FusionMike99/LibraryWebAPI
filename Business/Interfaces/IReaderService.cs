using Business.Models;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IReaderService : ICrud<ReaderModel>
    {
        IEnumerable<ReaderModel> GetReadersThatDontReturnBooks();
    }
}