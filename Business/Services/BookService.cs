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
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            mapper = new Mapper(configuration);

        }

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

        }

        public async Task AddAsync(BookModel model)
        {
            if (!ValidBookModel(model))
            {
                throw new LibraryException("Something wrong");
            }

            var element = mapper.Map<BookModel, Book>(model);
            await unitOfWork.BookRepository.AddAsync(element);
            await unitOfWork.SaveAsync();
            model.Id = element.Id;
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await unitOfWork.BookRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public IEnumerable<BookModel> GetAll()
        {
            var elements = unitOfWork.BookRepository.FindAll();
            var models = mapper.Map<IQueryable<Book>, IEnumerable<BookModel>>(elements);
            return models;
        }

        public IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch)
        {
            var elements = unitOfWork.BookRepository.FindAllWithDetails();
            elements = elements
                .Where(e => e.Author == filterSearch.Author || e.Year == filterSearch.Year);
            var models = mapper.Map<IQueryable<Book>, IEnumerable<BookModel>>(elements);
            return models;
        }

        public async Task<BookModel> GetByIdAsync(int id)
        {
            var element = await unitOfWork.BookRepository.GetByIdWithDetailsAsync(id);
            var model = mapper.Map<Book, BookModel>(element);
            return model;
        }

        public async Task UpdateAsync(BookModel model)
        {
            if (!ValidBookModel(model))
            {
                throw new LibraryException("Something wrong");
            }

            var element = mapper.Map<BookModel, Book>(model);
            unitOfWork.BookRepository.Update(element);
            await unitOfWork.SaveAsync();
        }

        private bool ValidBookModel(BookModel model)
        {
            if (string.IsNullOrEmpty(model.Title)
                || string.IsNullOrEmpty(model.Author)
                || model.Year > DateTime.Now.Year)
            {
                return false;
            }
            return true;
        }
    }
}
