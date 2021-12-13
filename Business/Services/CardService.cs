using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            mapper = new Mapper(configuration);

        }

        public CardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

        }

        public async Task AddAsync(CardModel model)
        {
            model.Created = DateTime.Now;
            var element = mapper.Map<CardModel, Card>(model);
            await unitOfWork.CardRepository.AddAsync(element);
            await unitOfWork.SaveAsync();
            model.Id = element.Id;
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await unitOfWork.CardRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public IEnumerable<CardModel> GetAll()
        {
            var elements = unitOfWork.CardRepository.FindAllWithDetails();
            var models = mapper.Map<IQueryable<Card>, IEnumerable<CardModel>>(elements);
            return models;
        }

        public IEnumerable<BookModel> GetBooksByCardId(int cardId)
        {
            var books = unitOfWork.BookRepository.FindAllWithDetails();
            var elements = books.Where(x => x.Cards.Any(c => c.CardId == cardId));
            var model = mapper.Map<IQueryable<Book>, IEnumerable<BookModel>>(elements);
            return model;
        }

        public async Task<CardModel> GetByIdAsync(int id)
        {
            var element = await unitOfWork.CardRepository.GetByIdWithDetailsAsync(id);
            var model = mapper.Map<Card, CardModel>(element);
            return model;
        }

        public async Task HandOverBookAsync(int cartId, int bookId)
        {
            var histories = unitOfWork.HistoryRepository.FindAll();
            var element = histories
                .FirstOrDefault(h => h.CardId == cartId && h.BookId == bookId);

            if (element == null)
            {
                throw new LibraryException("History isn't found");
            }

            if (element.ReturnDate != default)
            {
                throw new LibraryException("Book is already returned");
            }

            element.ReturnDate = DateTime.Now;
            unitOfWork.HistoryRepository.Update(element);
            await unitOfWork.SaveAsync();
        }

        public async Task TakeBookAsync(int cartId, int bookId)
        {
            var histories = unitOfWork.HistoryRepository.FindAll();
            var history = histories
                .FirstOrDefault(h => h.BookId == bookId && h.ReturnDate > DateTime.Now);

            if (history != null)
            {
                throw new LibraryException("Book isn't returned");
            }

            var card = await unitOfWork.CardRepository.GetByIdAsync(cartId);
            var book = await unitOfWork.BookRepository.GetByIdAsync(bookId);

            if (card is null || book is null)
            {
                throw new LibraryException("Not exist card or book");
            }

            var element = new History
            {
                CardId = cartId,
                BookId = bookId,
                Card = card,
                Book = book,
                TakeDate = DateTime.Now
            };

            await unitOfWork.HistoryRepository.AddAsync(element);
            card.Books.Add(element);
            book.Cards.Add(element);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(CardModel model)
        {
            var element = mapper.Map<CardModel, Card>(model);
            unitOfWork.CardRepository.Update(element);
            await unitOfWork.SaveAsync();
        }
    }
}
