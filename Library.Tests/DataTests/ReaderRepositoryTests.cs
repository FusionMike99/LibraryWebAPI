using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Library.Tests.DataTests
{
    [TestFixture]
    public class ReaderRepositoryTests
    {
        private DbContextOptions<LibraryDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = UnitTestHelper.GetUnitTestDbOptions();
        }

        [Test]
        public void ReaderRepository_GetAllWithDetails_ReturnAllValues()
        {
            using (var context = new LibraryDbContext(_options))
            {
                //arrange
                var readerRepository = new ReaderRepository(context);

                //act
                var readers = readerRepository.GetAllWithDetails().ToList();

                //assert
                Assert.AreEqual(2, readers.Count);
                Assert.IsNotNull(readers[0].ReaderProfile);
                Assert.AreEqual("The night's watch", readers[0].ReaderProfile.Address);
                Assert.AreEqual("golub", readers[0].ReaderProfile.Phone);
            }
        }

        [Test]
        public async Task ReaderRepository_GetByIdWithDetails_ReturnValueById()
        {
            using (var context = new LibraryDbContext(_options))
            {
                //arrange
                int id = 1;
                var readerRepository = new ReaderRepository(context);

                //act
                var reader = await readerRepository.GetByIdWithDetails(id);

                //assert
                Assert.IsNotNull(reader);
                Assert.AreEqual("Jon Snow", reader.Name);
                Assert.AreEqual("jon_snow@epam.com", reader.Email);
                Assert.AreEqual("The night's watch", reader.ReaderProfile.Address);
                Assert.AreEqual("golub", reader.ReaderProfile.Phone);
            }
        }
    }
}
