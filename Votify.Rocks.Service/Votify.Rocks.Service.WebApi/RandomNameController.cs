using System;
using System.Web.Http;

namespace Votify.Rocks.Service.WebApi
{
    [RoutePrefix("RandomName")]
    public class RandomNameController : ApiController
    {
        private readonly IRandomNameGeneratorService _randomNameGeneratorService;

        public RandomNameController(IRandomNameGeneratorService randomNameGeneratorService)
        {
            if (randomNameGeneratorService == null) throw new ArgumentNullException(nameof(randomNameGeneratorService));
            _randomNameGeneratorService = randomNameGeneratorService;
        }

        /// <summary>
        /// Generates a random participant name
        /// </summary>
        /// <returns></returns>
        [Route("Generate")]
        [HttpGet]
        public string RandomName()
        {
            return _randomNameGeneratorService.Generate();
        }
    }
}
