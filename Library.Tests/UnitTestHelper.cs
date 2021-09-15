using System;
using AutoMapper;
using Business;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<LibraryDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                SeedData(context);
            }
            return options;
        }

        public static void SeedData(LibraryDbContext context)
        {
            context.Books.Add(new Book { Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996 });
            context.Books.Add(new Book { Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994 });
            context.Cards.Add(new Card { Id = 1, ReaderId = 1, Created = new DateTime(2020, 7, 21) });
            context.Readers.Add(new Reader { Id = 1, Name = "Jon Snow", Email = "jon_snow@epam.com" });
            context.Readers.Add(new Reader { Id = 2, Name = "Night King", Email = "night_king@gmail.com"});
            context.ReaderProfiles.Add(new ReaderProfile { ReaderId = 1, Phone = "golub", Address = "The night's watch" });
            context.ReaderProfiles.Add(new ReaderProfile { ReaderId = 2, Phone = "telepathy", Address = "North" });
            context.Histories.Add(new History { BookId = 1, CardId = 1, Id = 1, TakeDate = new DateTime(2020, 7, 22), ReturnDate = new DateTime(2020, 7, 23) });

            //for method GetReadersThatDontReturnBooks
            //ok, I'll use it also for cards integration tests
            // will change everything with dates, because it wont be unit tests
            //do you understand that changing dates always make me change my test data in the integration tests because of GetAll method?
            context.Cards.Add(new Card { Id = 2, ReaderId = 2, Created = new DateTime(2020, 7, 23) });
            context.Histories.Add(new History { Id = 2, BookId = 2, CardId = 2, TakeDate = new DateTime(2020, 7, 23) });
            context.SaveChanges();
        }

        public static Mapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}