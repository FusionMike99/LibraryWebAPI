using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Tests.IntegrationTests
{
    [TestFixture]
    public class StatisticIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private BookModelEqualityComparer _bookModelComparer;
        private ReaderActivityModelEqualityComparer _readerActivityModelComparer;
        private readonly string requestUri = "api/statistic/";

        [OneTimeSetUp]
        public void Init()
        {
            _bookModelComparer = new BookModelEqualityComparer();
            _readerActivityModelComparer = new ReaderActivityModelEqualityComparer();
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        //2020-07-24
        [Test, Order(0)]
        public async Task HistoryController_GetMostPopularBooks()
        {
            var httpResponse = await _client.GetAsync(requestUri + "popularBooks?bookCount=2");


            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            httpResponse.EnsureSuccessStatusCode();
            var actual = JsonConvert.DeserializeObject<IEnumerable<BookModel>>(stringResponse).ToList();

            Assert.That(actual.OrderBy(x => x.Id),
                Is.EqualTo(ExpectedMostPopularBooks).Using(_bookModelComparer));
        }

        [Test, Order(0)]
        public async Task HistoryController_GetReadersWhoTookTheMostBooks()
        {
            var httpResponse = await _client.GetAsync($"{requestUri}biggestReaders?readersCount=2&" +
                                                      "firstDate=2020-7-21&" +
                                                      "lastDate=2020-7-24");

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            httpResponse.EnsureSuccessStatusCode();
            var actual = JsonConvert.DeserializeObject<IEnumerable<ReaderActivityModel>>(stringResponse).ToList();

            Assert.That(actual.OrderBy(x => x.ReaderId),
                Is.EqualTo(ExpectedReadersWhoTookTheMostBooks)
                    .Using(_readerActivityModelComparer));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        private IEnumerable<BookModel> ExpectedMostPopularBooks =>
            new[]
            {
                new BookModel {Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new BookModel {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994},
            };

        private IEnumerable<ReaderActivityModel> ExpectedReadersWhoTookTheMostBooks =>
            new[]
            {
                new ReaderActivityModel{ ReaderId = 1, BooksCount = 1, ReaderName = "Jon Snow" } 
            };
    }
}