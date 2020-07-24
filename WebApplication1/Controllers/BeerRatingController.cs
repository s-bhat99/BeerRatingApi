using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BeerRatingController : ControllerBase
    {
        private IBeerRatingRepository _beerRepo;
        
        private readonly ILogger<BeerRatingController> _logger;

        public BeerRatingController(ILogger<BeerRatingController> logger, IBeerRatingRepository beerRatingRepository)
        {
            _logger = logger;
            _beerRepo = beerRatingRepository;
        }

        /// <summary>
        /// Gets the Beer Details based on the name of the Beer - Task 2
        /// </summary>
        /// <param name="name">Name of the Beer</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ActionName("GetBeerDetailsByName")]
        public async Task<IActionResult> GetBeerDetailsByName(string name)
        {
            if (name == null || name == string.Empty)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The name of the Beer is not supplied!"),
                    ReasonPhrase = "Errors in the request!"
                };
                _logger.LogError("The name of the Beer is not supplied!");
                return BadRequest(errorResponse);
            }


            return Ok(await _beerRepo.GetBeerDetails(name));

        }

        /// <summary>
        /// Adds the user rating for a Beer based on the given beer id  - Task 1
        /// </summary>
        /// <param name="id">Beer Id</param>
        /// <param name="userRating">User Name, Comments and Rating</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ActionName("AddRating")]

        public async Task<IActionResult> AddRating(int id, [FromBody] UserRating userRating)
        {
            var validationErrors = new List<ParameterValidationError>();

            if (! await _beerRepo.IsValidId(id))
            {
                validationErrors.Add(new ParameterValidationError("id", "The Beer with id "+id+" doesn't exist"));
            }
            
            if (userRating.Rating >5 || userRating.Rating < 1)
            {
                validationErrors.Add(new ParameterValidationError("rating", "Rating should be between 1 and 5"));
            }

            if (validationErrors.Any())
            {
                string errorStr = null;
                foreach(var item in validationErrors)
                {
                    errorStr = string.Concat(errorStr ?? string.Empty, " ",item.Reason);
                }
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(errorStr),
                    ReasonPhrase = "Errors in the request!"
                };
                return BadRequest(errorResponse);
            }

            //add to the file
            return Ok(_beerRepo.SaveRatings(id, userRating));
        }
    }
}
