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
    public class ReadersController : ControllerBase
    {
        private readonly IReaderService readerService;

        public ReadersController(IReaderService readerService)
        {
            this.readerService = readerService;
        }

        //GET: /api/readers/
        [HttpGet]
        public ActionResult<IEnumerable<ReaderModel>> Get()
        {
            try
            {
                var result = readerService.GetAll();
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

        //GET: /api/readers/dontreturnbooks
        [HttpGet]
        [Route("[action]")]
        public ActionResult<IEnumerable<ReaderModel>> DontReturnBooks()
        {
            try
            {
                var result = readerService.GetReadersThatDontReturnBooks();
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

        //GET: /api/readers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ReaderModel>>> GetById(int id)
        {
            try
            {
                var result = await readerService.GetByIdAsync(id);
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

        //POST: /api/readers/
        [HttpPost("{id?}")]
        public async Task<ActionResult> Add([FromBody] ReaderModel readerModel)
        {
            if (readerModel == null)
            {
                return BadRequest();
            }
            try
            {
                await readerService.AddAsync(readerModel);
                return CreatedAtAction(nameof(Add), new { id = readerModel.Id }, readerModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT: /api/readers/
        [HttpPut]
        public async Task<ActionResult> Update(ReaderModel readerModel)
        {
            if (readerModel == null)
            {
                return BadRequest();
            }
            try
            {
                await readerService.UpdateAsync(readerModel);
                return Ok(readerModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE: /api/readers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await readerService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
