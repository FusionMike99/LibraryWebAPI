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
    public class CardsController : ControllerBase
    {
        private readonly ICardService cardService;

        public CardsController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        //GET: /api/cards/
        [HttpGet]
        public ActionResult<IEnumerable<CardModel>> Get()
        {
            try
            {
                var result = cardService.GetAll();
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

        //GET: /api/cards/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CardModel>>> GetById(int id)
        {
            try
            {
                var result = await cardService.GetByIdAsync(id);
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

        //POST: /api/cards/
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CardModel cardModel)
        {
            if (cardModel == null)
            {
                return BadRequest();
            }
            try
            {
                await cardService.AddAsync(cardModel);
                return Ok(cardModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT: /api/cards/
        [HttpPut]
        public async Task<ActionResult> Update(CardModel cardModel)
        {
            if (cardModel == null)
            {
                return BadRequest();
            }
            try
            {
                await cardService.UpdateAsync(cardModel);
                return Ok(cardModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE: /api/cards/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await cardService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: /api/cards/1/books
        [HttpGet("{id}/books")]
        public ActionResult<IEnumerable<CardModel>> GetBooksById(int id)
        {
            try
            {
                var result = cardService.GetBooksByCardId(id);
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

        //POST: /api/cards/1/books/1
        [HttpPost("{cardId}/books/{bookId}")]
        public async Task<ActionResult> TakeBook(int cardId, int bookId)
        {
            try
            {
                await cardService.TakeBookAsync(cardId, bookId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE: /api/cards/1
        [HttpDelete("{cardId}/books/{bookId}")]
        public async Task<ActionResult> HandOverBook(int cardId, int bookId)
        {
            try
            {
                await cardService.HandOverBookAsync(cardId, bookId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
