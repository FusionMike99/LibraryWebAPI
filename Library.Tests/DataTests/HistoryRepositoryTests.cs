using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Data.Repositories;
using NUnit.Framework;

namespace Library.Tests.DataTests
{
    [TestFixture]
    public class HistoryRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task HistoryRepository_GetById(int id)
        {

            await using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var historyRepository = new HistoryRepository(context);

            var history = await historyRepository.GetByIdAsync(id);

            var expected = ExpectedHistories.FirstOrDefault(x => x.Id == id);
            Assert.That(history,
                Is.EqualTo(expected).Using(new HistoryEqualityComparer()));
        }

        [Test]
        public async Task HistoryRepository_FindAll()
        {
            await using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var historyRepository = new HistoryRepository(context);

            var histories = historyRepository.FindAll();

            Assert.That(histories,
                Is.EqualTo(ExpectedHistories).Using(new HistoryEqualityComparer()));
        }

        [Test]
        public async Task HistoryRepository_AddAsync_()
        {
            await using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());
            
            var historyRepository = new HistoryRepository(context);
            var history = new History { Id = 3 };

            await historyRepository.AddAsync(history);
            await context.SaveChangesAsync();

            Assert.That(context.Histories.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task BookRepository_DeleteByIdAsync_DeletesEntity()
        {
            await using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());
            
            var historyRepository = new HistoryRepository(context);

            await historyRepository.DeleteByIdAsync(1);
            await context.SaveChangesAsync();

            Assert.That(context.Histories.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task BookRepository_Update_UpdatesEntity()
        {
            await using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());
            
            var historyRepository = new HistoryRepository(context);

            var history = new History
            {
                BookId = 2, CardId = 2, Id = 1, TakeDate = new DateTime(2020, 7, 20),
                ReturnDate = new DateTime(2020, 7, 21)
            };

            historyRepository.Update(history);
            await context.SaveChangesAsync();

            Assert.That(history, Is.EqualTo(
                new History { BookId = 2, CardId = 2, Id = 1, TakeDate = new DateTime(2020, 7, 20), ReturnDate = new DateTime(2020, 7, 21) })
                .Using(new HistoryEqualityComparer()));
        }

        [Test]
        public async Task HistoryRepository_GetAllWithDetails()
        {
            await using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());
            
            var historyRepository = new HistoryRepository(context);

            var histories = historyRepository.GetAllWithDetails();

            Assert.That(histories,
                Is.EqualTo(ExpectedHistories).Using(new HistoryEqualityComparer()));
            Assert.That(histories.Select(x => x.Card).OrderBy(x => x.Id),
                Is.EqualTo(ExpectedCards).Using(new CardEqualityComparer()));
            Assert.That(histories.Select(x => x.Card.Reader).OrderBy(x => x.Id),
                Is.EqualTo(ExpectedReaders).Using(new ReaderEqualityComparer()));
            Assert.That(histories.Select(x => x.Book).OrderBy(x => x.Id),
                Is.EqualTo(ExpectedBooks).Using(new BookEqualityComparer()));
        }

        private static IEnumerable<History> ExpectedHistories =>
            new[]
            {
                new History { BookId = 1, CardId = 1, Id = 1, TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) } ,
                new History { Id = 2, BookId = 2, CardId = 2, TakeDate = new DateTime(2020, 7, 23) }
            };

        private static IEnumerable<Card> ExpectedCards =>
            new[]
            {
                new Card { Id = 1, ReaderId = 1, Created = new DateTime(2020, 7, 21) },
                new Card { Id = 2, ReaderId = 2, Created = new DateTime(2020, 7, 23) }
            };

        private static IEnumerable<Reader> ExpectedReaders =>
            new[]
            {
                new Reader { Id = 1, Name = "Jon Snow", Email = "jon_snow@epam.com" },
                new Reader { Id = 2, Name = "Night King", Email = "night_king@gmail.com"}
            };

        private static IEnumerable<Book> ExpectedBooks =>
            new[]
            {
                new Book { Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996 },
                new Book { Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994 }
            };
    }
}