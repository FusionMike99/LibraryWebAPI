using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StatisticService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            mapper = new Mapper(configuration);

        }

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

        }

        public IEnumerable<BookModel> GetMostPopularBooks(int bookCount)
        {
            var histories = unitOfWork.HistoryRepository.GetAllWithDetails();
            var grouped = histories.GroupBy(h => h.BookId).Select(h => new
            {
                BookId = h.Key,
                BookCount = h.Count()
            })
                .OrderByDescending(m => m.BookCount)
                .Select(m => m.BookId)
                .Take(bookCount);

            var elements = grouped.Select(id => histories.First(h => h.BookId == id).Book);

            var models = mapper.Map<IQueryable<Book>, IEnumerable<BookModel>>(elements);
            return models;
        }

        public IEnumerable<ReaderActivityModel> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate)
        {
            var histories = unitOfWork.HistoryRepository.GetAllWithDetails()
                .Where(h => h.ReturnDate >= firstDate && h.ReturnDate <= lastDate)
                .ToList();

            var models = histories
                .GroupBy(h => h.Card.ReaderId)
                .OrderByDescending(g => g.Count())
                .Take(readersCount)
                .Select(m => new ReaderActivityModel
                {
                    ReaderId = m.Key,
                    BooksCount = m.Count(),
                    ReaderName = m.ElementAt(0).Card.Reader.Name
                });

            return models;
        }
    }
}
