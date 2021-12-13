using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        //GET: /api/statistic/popularBooks?bookCount=2
        [Route("[action]")]
        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> PopularBooks([FromQuery] int bookCount)
        {
            try
            {
                var result = statisticService.GetMostPopularBooks(bookCount);
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

        //GET: /api/statistic/biggestReaders?readersCount=2&firstDate=2020-7-21&lastDate=2020-7-24
        [Route("[action]")]
        [HttpGet]
        public ActionResult<IEnumerable<ReaderActivityModel>> BiggestReaders([FromQuery] int readersCount,
            DateTime firstDate, DateTime lastDate)
        {
            try
            {
                var result = statisticService.GetReadersWhoTookTheMostBooks(readersCount - 1, firstDate, lastDate);
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
    }
}
