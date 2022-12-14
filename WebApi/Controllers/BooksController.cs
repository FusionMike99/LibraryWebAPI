using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        //GET: /api/books/?Author=Jon%20Snow&Year=1996
        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> GetByFilter([FromQuery] FilterSearchModel model)
        {
            try
            {
                IEnumerable<BookModel> result;
                if (string.IsNullOrEmpty(model.Author) || model.Year < 1)
                {
                    result = bookService.GetAll();
                }
                else
                {
                    result = bookService.GetByFilter(model);
                }
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: /api/books/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetById(int id)
        {
            try
            {
                var result = await bookService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST: /api/books/
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BookModel bookModel)
        {
            if (bookModel == null)
            {
                return BadRequest();
            }
            try
            {
                await bookService.AddAsync(bookModel);
                return CreatedAtAction(nameof(Add), new { bookModel.Id }, bookModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT: /api/books/
        [HttpPut]
        public async Task<ActionResult> Update(BookModel bookModel)
        {
            if (bookModel == null)
            {
                return BadRequest();
            }
            try
            {
                await bookService.UpdateAsync(bookModel);
                return Ok(bookModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE: /api/books/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await bookService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}