using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Library.Tests.BusinessTests
{
    [TestFixture]
    public class HistoryServiceTests
    {
        [Test]
        public void HistoryService_GetMostPopularBooks()
        {
            var actual = MockedStatisticService.GetMostPopularBooks(5).ToList();

            Assert.That(actual, Is.InstanceOf<IEnumerable<BookModel>>());
            Assert.That(actual.OrderBy(x => x.Id), 
                Is.EqualTo(ExpectedMostPopularBooks).Using(new BookModelEqualityComparer()));
        }

        [Test]
        public void HistoryService_GetReadersWhoTookTheMostBooks()
        {
            var actual = MockedStatisticService.GetReadersWhoTookTheMostBooks(5,
                new DateTime(2020, 7, 21),
                new DateTime(2020, 7, 24));

            Assert.That(actual, Is.InstanceOf<IEnumerable<ReaderActivityModel>>());
            Assert.That(actual.OrderBy(x => x.ReaderId),
                Is.EqualTo(ExpectedReadersWhoTookTheMostBooks)
                    .Using(new ReaderActivityModelEqualityComparer()));
        }

        private static IStatisticService MockedStatisticService
        {
            get
            {
                var mockUnitOfWork = new Mock<IUnitOfWork>();
                mockUnitOfWork
                    .Setup(m => m.HistoryRepository.GetAllWithDetails())
                    .Returns(Histories.AsQueryable());
                return new StatisticService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            }
        }

        private static IEnumerable<History> Histories =>
            new[]
            {
                new History { Id = 1, BookId = 1, CardId = 1, Book = Books.ElementAt(0), Card = Cards.ElementAt(0), 
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 2, BookId = 1, CardId = 3, Book = Books.ElementAt(0), Card = Cards.ElementAt(2),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 3, BookId = 1, CardId = 4, Book = Books.ElementAt(0), Card = Cards.ElementAt(3),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 4, BookId = 1, CardId = 5, Book = Books.ElementAt(0), Card = Cards.ElementAt(4),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 5, BookId = 2, CardId = 2, Book = Books.ElementAt(1), Card = Cards.ElementAt(1),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 6, BookId = 3, CardId = 3, Book = Books.ElementAt(2), Card = Cards.ElementAt(2),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 7, BookId = 4, CardId = 6, Book = Books.ElementAt(3), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 8, BookId = 5, CardId = 4, Book = Books.ElementAt(4), Card = Cards.ElementAt(3),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 9, BookId = 5, CardId = 3, Book = Books.ElementAt(4), Card = Cards.ElementAt(2),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 10, BookId = 5, CardId = 6, Book = Books.ElementAt(4), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 11, BookId = 6, CardId = 1, Book = Books.ElementAt(5), Card = Cards.ElementAt(0),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 12, BookId = 6, CardId = 8, Book = Books.ElementAt(5), Card = Cards.ElementAt(7),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 13, BookId = 6, CardId = 6, Book = Books.ElementAt(5), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 14, BookId = 7, CardId = 4, Book = Books.ElementAt(6), Card = Cards.ElementAt(3),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 15, BookId = 7, CardId = 3, Book = Books.ElementAt(6), Card = Cards.ElementAt(2),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 16, BookId = 7, CardId = 6, Book = Books.ElementAt(6), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 17, BookId = 8, CardId = 6, Book = Books.ElementAt(7), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 18, BookId = 9, CardId = 6, Book = Books.ElementAt(8), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 19, BookId = 10, CardId = 6, Book = Books.ElementAt(9), Card = Cards.ElementAt(5),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) },
                new History { Id = 20, BookId = 10, CardId = 7, Book = Books.ElementAt(9), Card = Cards.ElementAt(6),
                    TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) }
            };

        private static IEnumerable<Card> Cards =>
            new[]
            {
                new Card {Id = 1, ReaderId = 1, Reader = Readers.ElementAt(0), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 2, ReaderId = 1, Reader = Readers.ElementAt(0), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 3, ReaderId = 3, Reader = Readers.ElementAt(2), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 4, ReaderId = 4, Reader = Readers.ElementAt(3), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 5, ReaderId = 5, Reader = Readers.ElementAt(4), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 6, ReaderId = 6, Reader = Readers.ElementAt(5), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 7, ReaderId = 1, Reader = Readers.ElementAt(0), Created = new DateTime(2020, 6, 21)},
                new Card {Id = 8, ReaderId = 5, Reader = Readers.ElementAt(4), Created = new DateTime(2020, 6, 21)}
            };

        private static IEnumerable<Reader> Readers =>
            new[]
            {
                new Reader {Id = 1, Name = "Jon Snow", Email = "jon_snow@epam.com"},
                new Reader {Id = 2, Name = "Night King", Email = "night_king@gmail.com"},
                new Reader {Id = 3, Name = "Daenerys Targaryen", Email = "daenerys_targaryen@epam.com"},
                new Reader {Id = 4, Name = "Arya Stark", Email = "arya_stark@gmail.com"},
                new Reader {Id = 5, Name = "Theon Greyjoy", Email = "theon_greyjoy@epam.com"},
                new Reader {Id = 6, Name = "Tyrion Lannister", Email = "tyrion_lannister@gmail.com"}
            };

        private static IEnumerable<Book> Books =>
            new[]
            {
                new Book {Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new Book {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994},
                new Book {Id = 3, Author = "J.R.R. Tolkien", Title = "The Fellowship of the Ring", Year = 1954},
                new Book {Id = 4, Author = "Mary Shelley", Title = "Frankenstein", Year = 1818},
                new Book {Id = 5, Author = "Frank Herbert", Title = "Dune", Year = 1965},
                new Book {Id = 6, Author = "Andy Weir", Title = "The Martian", Year = 2015},
                new Book {Id = 7, Author = "Aldous Huxley", Title = "Brave New World", Year = 1932},
                new Book {Id = 8, Author = "Ray Bradbury", Title = "Fahrenheit 451", Year = 1953},
                new Book {Id = 9, Author = "Philip Pullman", Title = "His Dark Materials", Year = 1995},
                new Book
                {
                    Id = 10, Author = "Douglas Adams", Title = "The Hitchhiker's Guide To The Galaxy", Year = 1978
                }
            };

        private static IEnumerable<BookModel> ExpectedMostPopularBooks =>
            new[]
            {
                new BookModel {Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new BookModel {Id = 5, Author = "Frank Herbert", Title = "Dune", Year = 1965},
                new BookModel {Id = 6, Author = "Andy Weir", Title = "The Martian", Year = 2015},
                new BookModel {Id = 7, Author = "Aldous Huxley", Title = "Brave New World", Year = 1932},
                new BookModel
                    {Id = 10, Author = "Douglas Adams", Title = "The Hitchhiker's Guide To The Galaxy", Year = 1978}
            };

        private static IEnumerable<ReaderActivityModel> ExpectedReadersWhoTookTheMostBooks =>
            new[]
            {
                new ReaderActivityModel{ReaderId = 1, BooksCount = 4, ReaderName = "Jon Snow"},
                new ReaderActivityModel{ReaderId = 3, BooksCount = 4, ReaderName = "Daenerys Targaryen"},
                new ReaderActivityModel{ReaderId = 4, BooksCount = 3, ReaderName = "Arya Stark"},
                new ReaderActivityModel{ReaderId = 5, BooksCount = 2, ReaderName = "Theon Greyjoy"},
                new ReaderActivityModel{ReaderId = 6, BooksCount = 7, ReaderName = "Tyrion Lannister"},
            };
    }
}