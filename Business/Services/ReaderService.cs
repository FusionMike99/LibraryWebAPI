using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ReaderService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            mapper = new Mapper(configuration);

        }

        public ReaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

        }

        public async Task AddAsync(ReaderModel model)
        {
            if (!ValidReaderModel(model))
            {
                throw new LibraryException("Something wrong");
            }

            var element = mapper.Map<ReaderModel, Reader>(model);
            await unitOfWork.ReaderRepository.AddAsync(element);
            await unitOfWork.SaveAsync();
            model.Id = element.Id;
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await unitOfWork.ReaderRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public IEnumerable<ReaderModel> GetAll()
        {
            var elements = unitOfWork.ReaderRepository.GetAllWithDetails();
            var models = mapper.Map<IQueryable<Reader>, IEnumerable<ReaderModel>>(elements);
            return models;
        }

        public async Task<ReaderModel> GetByIdAsync(int id)
        {
            var element = await unitOfWork.ReaderRepository.GetByIdWithDetails(id);
            var model = mapper.Map<Reader, ReaderModel>(element);
            return model;
        }

        public IEnumerable<ReaderModel> GetReadersThatDontReturnBooks()
        {
            var histories = unitOfWork.HistoryRepository.FindAll();
            var cardsIds = histories.Where(h => h.ReturnDate == default)
                .Select(h => h.CardId);

            var cards = unitOfWork.CardRepository.FindAll();
            var readersIds = cardsIds.Select(id => cards.First(c => c.Id == id).ReaderId);

            var readers = unitOfWork.ReaderRepository.GetAllWithDetails();
            var filtersReaders = readersIds
                .Select(id => readers.First(r => r.Id == id))
                .OrderBy(r => r.Id);

            var models = mapper.Map<IOrderedQueryable<Reader>, IEnumerable<ReaderModel>>(filtersReaders);
            return models;
        }

        public async Task UpdateAsync(ReaderModel model)
        {
            if (!ValidReaderModel(model))
            {
                throw new LibraryException("Something wrong");
            }

            var element = mapper.Map<ReaderModel, Reader>(model);
            unitOfWork.ReaderRepository.Update(element);
            await unitOfWork.SaveAsync();
        }

        private bool ValidReaderModel(ReaderModel model)
        {
            if (string.IsNullOrEmpty(model.Name)
                || string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.Phone)
                || string.IsNullOrEmpty(model.Address))
            {
                return false;
            }
            return true;
        }
    }
}
