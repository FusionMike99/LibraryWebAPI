using Business.Models;
using System;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IStatisticService
    {
        IEnumerable<BookModel> GetMostPopularBooks(int bookCount);

        IEnumerable<ReaderActivityModel> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate);
    }
}