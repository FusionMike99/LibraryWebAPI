using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //Inject book service via constructor
        
        //GET: /api/books/?Author=Jon%20Snow&Year=1996
        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> GetByFilter([FromQuery] FilterSearchModel model)
        {
            throw new NotImplementedException();
        }
        
        //GET: /api/books/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        //POST: /api/books/
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BookModel bookModel)
        {
            throw new NotImplementedException();
        }

        //PUT: /api/books/
        [HttpPut]
        public async Task<ActionResult> Update(BookModel bookModel)
        {
            throw new NotImplementedException();
        }

        //DELETE: /api/books/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}