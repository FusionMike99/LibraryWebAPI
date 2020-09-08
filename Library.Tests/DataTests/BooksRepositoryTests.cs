using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Data.Repositories;
using NUnit.Framework;

namespace Library.Tests.DataTests
{
    [TestFixture]
    public class BooksRepositoryTests
    {
        [Test]
        public void BookRepository_FindAll_ReturnsAllValues()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var booksRepository = new BookRepository(context);

                var books = booksRepository.FindAll();

                Assert.AreEqual(2, books.Count());
            }
        }

        [Test]
        public async Task BookRepository_GetById_ReturnsSingleValue()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var booksRepository = new BookRepository(context);

                var book = await booksRepository.GetByIdAsync(1);

                Assert.AreEqual(1, book.Id);
                Assert.AreEqual("Jon Snow", book.Author);
                Assert.AreEqual("A song of ice and fire", book.Title);
                Assert.AreEqual(1996, book.Year);
            }
        }

        [Test]
        public async Task BookRepository_AddAsync_AddsValueToDatabase()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var booksRepository = new BookRepository(context);
                var book = new Book(){Id = 3};

                await booksRepository.AddAsync(book);
                await context.SaveChangesAsync();
                
                Assert.AreEqual(3, context.Books.Count());
            }
        }

        [Test]
        public async Task BookRepository_DeleteByIdAsync_DeletesEntity()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var bookRepository = new BookRepository(context);
                
                await bookRepository.DeleteByIdAsync(1);
                await context.SaveChangesAsync();
                
                Assert.AreEqual(1, context.Books.Count());
            }
        }

        [Test]
        public async Task BookRepository_Update_UpdatesEntity()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var booksRepository = new BookRepository(context);

                var book = new Book(){ Id = 1, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994};

                booksRepository.Update(book);
                await context.SaveChangesAsync();

                Assert.AreEqual(1, book.Id);
                Assert.AreEqual("John Travolta", book.Author);
                Assert.AreEqual("Pulp Fiction", book.Title);
                Assert.AreEqual(1994, book.Year);
            }
        }

        [Test]
        public async Task BooksRepository_GetByIdWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var expectedCardsInBook = 1;
                var booksRepository = new BookRepository(context);
                var bookWithIncludes = await booksRepository.GetByIdWithDetailsAsync(1);
                
                var actual = bookWithIncludes.Cards.Count;
                
                Assert.AreEqual(expectedCardsInBook, actual);
            }
        }

        [Test]
        public void BooksRepository_FindAllWithDetails_ReturnsWithIncludedEntities()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var expectedCardsInBook = 1;
                var booksRepository = new BookRepository(context);
                var bookWithIncludes = booksRepository.FindAllWithDetails();

                var actual = bookWithIncludes.FirstOrDefault().Cards.Count;
                
                Assert.AreEqual(expectedCardsInBook, actual);
            }
        }
    }
}