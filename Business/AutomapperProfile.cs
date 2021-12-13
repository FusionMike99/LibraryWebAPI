using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(p => p.CardsIds, b => b.MapFrom(book => book.Cards.Select(x => x.CardId)))
                .ReverseMap();

            CreateMap<Card, CardModel>()
                .ForMember(p => p.BooksIds, c => c.MapFrom(card => card.Books.Select(x => x.BookId)))
                .ReverseMap();

            CreateMap<Reader, ReaderModel>()
                .ForMember(p => p.Id, r => r.MapFrom(reader => reader.ReaderProfile.ReaderId))
                .ForMember(p => p.Phone, r => r.MapFrom(reader => reader.ReaderProfile.Phone))
                .ForMember(p => p.Address, r => r.MapFrom(reader => reader.ReaderProfile.Address))
                .ForMember(p => p.CardsIds, r => r.MapFrom(reader => reader.Cards.Select(x => x.Id)))
                .ReverseMap();
        }
    }
}