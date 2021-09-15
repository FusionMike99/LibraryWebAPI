using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Business.Models;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Tests.IntegrationTests
{
    [TestFixture]
    public class ReaderIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private ReaderModelEqualityComparer _comparer;
        private readonly string requestUri = "api/readers/";

        [SetUp]
        public void Init()
        {
            _comparer = new ReaderModelEqualityComparer();
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task ReaderController_GetAll_ReturnAllFromDb()
        {
            // arrange 
            var expected = GetReaderModels().ToList();
            var expectedLength = expected.Count;

            // act
            var httpResponse = await _client.GetAsync(requestUri);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<ReaderModel>>(stringResponse).ToList();
            Assert.AreEqual(expectedLength, actual.Count);
            for (int i = 0; i < expectedLength; i++)
            {
                Assert.IsTrue(_comparer.Equals(expected[i], actual[i]));
            }
        }

        [Test]
        public async Task ReaderController_GetByIdAsync_ReturnOneReader()
        {
            // arrange 
            var expected = GetReaderModels().First();
            var readerId = 1;

            // act
            var httpResponse = await _client.GetAsync(requestUri + readerId);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<ReaderModel>(stringResponse);
            Assert.IsTrue(_comparer.Equals(expected, actual));
        }

        [Test]
        public async Task ReaderController_GetByIdAsync_ReturnNotFound()
        {
            // arrange 
            var readerId = 999;

            // act
            var httpResponse = await _client.GetAsync(requestUri + readerId);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task ReaderController_GetReadersThatDontReturnBooks_ReturnReader()
        {
            // arrange 
            var expected = GetReaderModels().Last();

            // act
            var httpResponse = await _client.GetAsync(requestUri + "DontReturnBooks");

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<ReaderModel>>(stringResponse).First();
            Assert.IsTrue(_comparer.Equals(expected, actual));
        }

        [Test]
        public async Task ReaderController_Add_AddReaderToDb()
        {
            // arrange
            var reader = new ReaderModel 
            { 
                Name = "Scrooge McDuck", 
                Email = "only_money@gmail.com", 
                Phone = "999999999", 
                Address = "Glasgow" 
            };
            var content = new StringContent(JsonConvert.SerializeObject(reader), Encoding.UTF8, "application/json");
            
            // act
            var httpResponse = await _client.PostAsync(requestUri, content);
            var readerId = Int32.Parse(httpResponse.Headers.Location.Segments[3]);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            await CheckReaderInfoIntoDb(reader, readerId, 3);
        }

        [Test]
        public async Task ReaderController_Add_ThrowsExceptionIfModelIsIncorrect()
        {
            // Name is empty
            var reader = new ReaderModel { Name = "", Email = "only_money@gmail.com",
                Phone = "999999999", Address = "Glasgow" };
            await CheckExceptionWhileAddNewModel(reader);

            // Email is empty
            reader.Name = "Scrooge McDuck";
            reader.Email = "";
            await CheckExceptionWhileAddNewModel(reader);

            // Phone is empty
            reader.Email = "only_money@gmail.com";
            reader.Phone = "";
            await CheckExceptionWhileAddNewModel(reader);

            // Address is empty
            reader.Phone = "999999999";
            reader.Address = "";
            await CheckExceptionWhileAddNewModel(reader);
        }

        [Test]
        public async Task ReaderController_Update_UpdateReaderInDb()
        {
            // arrange
            var reader = new ReaderModel
            {
                Id = 1,
                Name = "Enzo Ferrari",
                Email = "scuderia_ferrari@gmail.com",
                Phone = "165479823",
                Address = "Modena, Maranello"
            };
            var content = new StringContent(JsonConvert.SerializeObject(reader), Encoding.UTF8, "application/json");

            // act
            var httpResponse = await _client.PutAsync(requestUri, content);

            //assert
            httpResponse.EnsureSuccessStatusCode();
            await CheckReaderInfoIntoDb(reader, reader.Id, 2);
        }

        [Test]
        public async Task ReaderController_Update_ThrowsExceptionIfModelIsIncorrect()
        {
            // Name is empty
            var reader = new ReaderModel { Id = 1, Name = "",  Email = "scuderia_ferrari@gmail.com",
                Phone = "165479823", Address = "Modena, Maranello" };
            await CheckExceptionWhileUpdateModel(reader);

            // Email is empty
            reader.Name = "Enzo Ferrari";
            reader.Email = "";
            await CheckExceptionWhileUpdateModel(reader);

            // Phone is empty
            reader.Email = "scuderia_ferrari@gmail.com";
            reader.Phone = "";
            await CheckExceptionWhileUpdateModel(reader);

            // Name is empty
            reader.Phone = "165479823";
            reader.Address = "";
            await CheckExceptionWhileUpdateModel(reader);
        }

        [Test]
        public async Task ReaderController_Delete_DeleteReaderFromDb()
        {
            // arrange
            var readerId = 2;

            // act
            var httpResponse = await _client.DeleteAsync(requestUri + readerId);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                Assert.AreEqual(1, context.Readers.Count());
            }
        }

        #region helpers
        private async Task CheckReaderInfoIntoDb(ReaderModel reader, int readerId, int expectedLength)
        {
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                Assert.AreEqual(expectedLength, context.Readers.Count());

                var dbReader = await context.Readers.FindAsync(readerId);
                Assert.AreEqual(readerId, dbReader.Id);
                Assert.AreEqual(reader.Name, dbReader.Name);
                Assert.AreEqual(reader.Email, dbReader.Email);

                var dbReaderProfile = await context.ReaderProfiles.FindAsync(readerId);
                Assert.AreEqual(readerId, dbReaderProfile.ReaderId);
                Assert.AreEqual(reader.Phone, dbReaderProfile.Phone);
                Assert.AreEqual(reader.Address, dbReaderProfile.Address);
            }
        }

        private async Task CheckExceptionWhileAddNewModel(ReaderModel reader)
        {
            var content = new StringContent(JsonConvert.SerializeObject(reader), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private async Task CheckExceptionWhileUpdateModel(ReaderModel reader)
        {
            var content = new StringContent(JsonConvert.SerializeObject(reader), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(requestUri, content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private IEnumerable<ReaderModel> GetReaderModels()
        {
            return new List<ReaderModel>()
            {
                new ReaderModel { Id = 1, Name = "Jon Snow", Email = "jon_snow@epam.com",
                    Phone = "golub", Address = "The night's watch" },
                new ReaderModel { Id = 2, Name = "Night King", Email = "night_king@gmail.com",
                    Phone = "telepathy", Address = "North" }
            };
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
