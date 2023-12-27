using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Models;
using CrudDapperGraphQL.Services;
using Moq;
using SendGrid.Helpers.Errors.Model;

namespace CrudDapperGraphQL.UnitTests.Services.Tests
{
    [TestClass]
    public class AuthorServiceTests
    {
        private readonly Mock<IRepository<Author, AuthorSave>> _repositoryMock = new Mock<IRepository<Author, AuthorSave>>();

        [TestMethod]
        public async Task GetAuthors_ReturnsAuthors()
        {
            // Arrange
            var service = new AuthorService(_repositoryMock.Object);
            var filter = new FilterModel();
            var expectedAuthor = new Author { Id = 1, Name = "Daniel", Surname = "Defoe", BooksJson = "[]" };

            _repositoryMock.Setup(repo => repo.GetAll(filter)).ReturnsAsync(new List<Author> { expectedAuthor });

            // Act
            var result = await service.GetAll(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.FirstOrDefault(), typeof(Author));
        }

        [TestMethod]
        public async Task GetAuthor_AuthorExists_ReturnsAuthor()
        {
            // Arrange
            var service = new AuthorService(_repositoryMock.Object);
            int authorId = 1;
            var expectedAuthor = new Author { Id = authorId, Name = "Daniel", Surname = "Defoe" , BooksJson = "[]" };

            _repositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync(expectedAuthor);

            // Act
            var result = await service.GetById(authorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAuthor, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetAuthor_AuthorNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var service = new AuthorService(_repositoryMock.Object);
            int authorId = -1;
            Author expectedAuthor = null;

            _repositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync(expectedAuthor);

            // Act and Assert
            await service.GetById(authorId); // throw Not Found Exception
        }

        [TestMethod]
        public async Task AuthorSave_Success_ReturnsSavedAuthor()
        {
            // Arrange
            var service = new AuthorService(_repositoryMock.Object);
            var authorToSave = new AuthorSave { Id = -1, Name = "Daniel", Surname = "Defoe" };

            var expectedSavedAuthor = new Author { 
                Id = authorToSave.Id.Value, 
                Name = authorToSave.Name, 
                Surname = authorToSave.Surname, 
                BooksJson = "[]" 
            };

            _repositoryMock.Setup(repo => repo.Save(authorToSave)).ReturnsAsync(expectedSavedAuthor);

            // Act
            var result = await service.Save(authorToSave);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSavedAuthor, result);
        }

        [TestMethod]
        public async Task AuthorDelete_Success_ReturnsTrue()
        {
            // Arrange
            var service = new AuthorService(_repositoryMock.Object);
            int authorId = -1;

            _repositoryMock.Setup(repo => repo.Delete(authorId)).ReturnsAsync(true);

            // Act
            var result = await service.Delete(authorId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
