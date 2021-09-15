using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Library.Tests.BusinessTests
{
    public class BooksServiceTests
    {
        [Test]
        public void BookService_GetAll_ReturnsBookModels()
        {
            var expected = GetTestBookModels().ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.FindAll())
                .Returns(GetTestBookEntities().AsQueryable);
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = bookService.GetAll().ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Author, actual[i].Author);
                Assert.AreEqual(expected[i].Title, actual[i].Title);
            }
        }
        
        private IEnumerable<BookModel> GetTestBookModels()
        {
            return new List<BookModel>()
            {
                new BookModel {Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909},
                new BookModel {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994},
                new BookModel {Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903},
                new BookModel {Id = 4, Author = "Robert Jordan", Title = "Lord of Chaos", Year = 1994}
            };
        }

        [Test]
        public async Task BookService_GetByIdAsync_ReturnsBookModel()
        {
            var expected = GetTestBookModels().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestBookEntities().First);
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = await bookService.GetByIdAsync(1);
            
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Author, actual.Author);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        private List<Book> GetTestBookEntities()
        {
            return new List<Book>()
            {
                new Book {Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909},
                new Book {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994},
                new Book {Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903},
                new Book {Id = 4, Author = "Robert Jordan", Title = "Lord of Chaos", Year = 1994}
            };
        }
        
        [Test]
        public async Task BookService_AddAsync_AddsModel()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var book = new BookModel {Id = 100, Author = "Honore de Balzac", Title = "The Splendors and Miseries of Courtesans"};
            
            //Act
            await bookService.AddAsync(book);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.AddAsync(It.Is<Book>(b => b.Author == book.Author && b.Id == book.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test] 
        public void BookService_AddAsync_ThrowsLibraryExceptionWithEmptyTitle()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var book = new BookModel {Id = 100, Author = "Honore de Balzac", Title = ""};
            
            Assert.ThrowsAsync<LibraryException>(() => bookService.AddAsync(book));
        }
        
        [Test] 
        public void BookService_AddAsync_ThrowsLibraryExceptionWithEmptyAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var book = new BookModel {Id = 100, Author = "", Title = "The Splendors and Miseries of Courtesans"};
            
            Assert.ThrowsAsync<LibraryException>(() => bookService.AddAsync(book));
        }
        
        [Test] 
        public void BookService_AddAsync_ThrowsLibraryExceptionWithInvalidYear()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var book = new BookModel {Id = 100, Author = "Honore de Balzac", Title = "The Splendors and Miseries of Courtesans", Year = 9999};
            
            Assert.ThrowsAsync<LibraryException>(() => bookService.AddAsync(book));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public async Task BookService_DeleteByIdAsync_DeletesBook(int bookId)
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.DeleteByIdAsync(It.IsAny<int>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.DeleteByIdAsync(bookId);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.DeleteByIdAsync(bookId), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
        
        [Test]
        public async Task BookService_UpdateAsync_UpdatesBook()
        {
            //Arrange
            var book = new BookModel{Id = 1, Author = "Honore de Balzac", Title = "Father Goriot"};
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.UpdateAsync(book);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.Update(It.Is<Book>(b => b.Author == book.Author && b.Id == book.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
        
        [Test]
        public void BookService_UpdateAsync_ThrowsLibraryExceptionWithEmptyAuthor()
        {
            //Arrange
            var book = new BookModel{Id = 1, Author = "", Title = "Father Goriot", Year = 1835};
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            Assert.ThrowsAsync<LibraryException>(() => bookService.UpdateAsync(book));
        }
        
        [Test]
        public void BookService_UpdateAsync_ThrowsLibraryExceptionWithEmptyTitle()
        {
            //Arrange
            var book = new BookModel{Id = 1, Author = "Honore de Balzac", Title = "", Year = 1835};
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            Assert.ThrowsAsync<LibraryException>(() => bookService.UpdateAsync(book));
        }
        
        [Test]
        public void BookService_UpdateAsync_ThrowsLibraryExceptionWithInvalidYear()
        {
            //Arrange
            var book = new BookModel{Id = 1, Author = "Honore de Balzac", Title = "Father Goriot", Year = 9999};
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            Assert.ThrowsAsync<LibraryException>(() => bookService.UpdateAsync(book));
        }

        [Test]
        public void BookService_GetByFilter_ReturnsBooksByAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.FindAllWithDetails()).Returns(GetTestBookEntities().AsQueryable);
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var filter = new FilterSearchModel{Author = "Jack London"};
            
            //Act
            var filteredBooks = bookService.GetByFilter(filter).ToList();
            
            Assert.AreEqual(2, filteredBooks.Count);
            foreach (var book in filteredBooks)
            {
                Assert.AreEqual(filter.Author, book.Author);
            }
        }
        
        [Test]
        public void BookService_GetByFilter_ReturnsBooksByYear()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.FindAllWithDetails()).Returns(GetTestBookEntities().AsQueryable);
            IBookService bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var filter = new FilterSearchModel{Year = 1994};
            
            var filteredBooks = bookService.GetByFilter(filter).ToList();
            
            Assert.AreEqual(2, filteredBooks.Count);
            foreach (var book in filteredBooks)
            {
                Assert.AreEqual(filter.Year, book.Year);
            }
        }
    }
}