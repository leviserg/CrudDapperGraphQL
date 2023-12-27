using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Models;
using CrudDapperGraphQL.Services;
using Moq;
using SendGrid.Helpers.Errors.Model;

namespace CrudDapperGraphQL.UnitTests.Services.Tests
{
    [TestClass]
    public class BookServiceTests
    {
        private static Mock<IRepository<Book, BookSave>> repositoryMock;
        private static List<Book> availableBooks;
        private static BookService service;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            availableBooks = new List<Book>()
            {
                new Book() { Id = 1, Title = "Jane Eyre", ReleaseDate = DateTime.Parse("1847-10-19 00:00:00"), AuthorsJson = "[]" },
                new Book() { Id = 2, Title = "Robinson Crusoe", ReleaseDate = DateTime.Parse("1719-04-25 00:00:00"), AuthorsJson = "[]" }
            };

            repositoryMock = new Mock<IRepository<Book, BookSave>>();
            service = new BookService(repositoryMock.Object);
        }

        [TestMethod]
        public async Task GetBooks_ReturnsBooks()
        {
            // Arrange
            var filter = new FilterModel();
            repositoryMock.Setup(repo => repo.GetAll(filter)).ReturnsAsync(availableBooks);

            // Act
            var result = await service.GetAll(filter);

            // Assert
            repositoryMock.Verify(x => x.GetAll(filter));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.LastOrDefault(), typeof(Book));
        }

        [TestMethod]
        public async Task GetBook_BookExists_ReturnsBook()
        {
            // Arrange
            Book expectedBook = availableBooks.OrderBy(x => x.Id).FirstOrDefault();
            repositoryMock.Setup(repo => repo.GetById(expectedBook.Id)).ReturnsAsync(expectedBook);

            // Act
            var result = await service.GetById(expectedBook.Id);

            // Assert
            repositoryMock.Verify(x => x.GetById(expectedBook.Id));
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedBook, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetBook_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            Book expectedBook = null;
            int bookId = -1;
            repositoryMock.Setup(repo => repo.GetById(bookId)).ReturnsAsync(expectedBook);

            // Act & Assert
            var result = await service.GetById(bookId);
        }

        [TestMethod]
        public async Task BookSave_Success_ReturnsSavedABook()
        {
            // Arrange
            Book savedBookExample = availableBooks.OrderBy(x => x.Id).FirstOrDefault();
            BookSave bookToSave = new BookSave { 
                Id = savedBookExample.Id,
                Title = savedBookExample.Title,
                ReleaseDate = savedBookExample.ReleaseDate,
                AuthorIds = new List<int>()
            };

            repositoryMock.Setup(repo => repo.Save(bookToSave)).ReturnsAsync(savedBookExample);

            // Act
            var result = await service.Save(bookToSave);

            // Assert
            repositoryMock.Verify(x => x.Save(bookToSave));
            Assert.IsNotNull(result);
            Assert.AreEqual(savedBookExample, result);
        }

        [TestMethod]
        public async Task BookDelete_Success_ReturnsTrue()
        {
            // Arrange
            int bookId = -1;
            repositoryMock.Setup(repo => repo.Delete(bookId)).ReturnsAsync(true);

            // Act
            var result = await service.Delete(bookId);

            // Assert
            repositoryMock.Verify(x => x.Delete(bookId));
            Assert.IsTrue(result);
        }
    }
}
