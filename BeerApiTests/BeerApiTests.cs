using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using WebApplication1.Repository;
using Moq;
using WebApplication1.Controllers;
using Microsoft.Extensions.Logging;
using WebApplication1.DTO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeerApiTests
{
    public class BeerApiTests
    {
        BeerRatingController controller;
        public BeerApiTests()
        {
            var mockRepo = new Mock<BeerRatingRepository>();
            ILogger<BeerRatingController> _logger = new Logger<BeerRatingController>(new LoggerFactory());

            controller = new BeerRatingController(_logger, mockRepo.Object);
        }

        [Fact]
        public async Task GetBeerDetailsByName_Success()
        {
            var result = await controller.GetBeerDetailsByName("he");


            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<BeerDetails>>(
                viewResult.Value);
            Assert.True(model.Count > 0);
        }

        [Fact]
        public async Task GetBeerDetailsByName_ReturnsBadRequestResult()
        {
            var result = await controller.GetBeerDetailsByName(null);


            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddRating_Success()
        {
            var result =await controller.AddRating(1, new UserRating { Comments = "Test", Rating = 1, UserName = "TestUser@test.com" });

            var viewResult = Assert.IsType<OkObjectResult>(result);            
        }


        [Fact]
        public async Task AddRating_BadRating()
        {
            var result = await controller.AddRating(1, new UserRating { Comments = "Test", Rating = -1, UserName = "TestUser@test.com" });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddRating_BadId()
        {
            var result = await controller.AddRating(0, new UserRating { Comments = "Test", Rating = 1, UserName = "TestUser@test.com" });

            Assert.IsType<BadRequestObjectResult>(result);
        }        
    }
}
