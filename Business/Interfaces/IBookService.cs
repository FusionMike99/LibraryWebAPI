using Business.Models;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IBookService : ICrud<BookModel>
    {
        IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch);
    }
}