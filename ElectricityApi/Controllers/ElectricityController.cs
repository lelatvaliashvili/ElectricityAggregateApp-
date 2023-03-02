using Electricity.Domain.Entities;
using Electricity.Domain.IService;
using Electricity.Domain.Models;
using Electricity.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ElectricityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricityController : ControllerBase
    {
        private readonly IElectricityConsumptionService _electricityConsumptionService;
        private readonly ILogger<ElectricityController> _logger;

        public ElectricityController(IElectricityConsumptionService electricityConsumptionService,
                                     ILogger<ElectricityController> logger)
        {
            _electricityConsumptionService = electricityConsumptionService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve the aggregated electricity data from database
        /// </summary>
        /// <returns>Return the list of aggregated data stored in data table</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ElectricityConsumptionAggregate>>> GetElectricityConsumptionsAsync()
        {
            _logger.LogInformation("Getting electricity consumption data...");

            var consumptions = await _electricityConsumptionService.GetAggregateConsumptionDataAsync();

            _logger.LogInformation("Electricity consumption data retrieved successfully.");

            return Ok(consumptions);
        }

        /// <summary>
        /// Add the parsed electricity data to database
        /// </summary>
        /// <returns>Return the list of aggregated data stored in data table</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ElectricityConsumptionAggregate>>> AggregateConsumptionData()
        {
            _logger.LogInformation("Aggregating electricity consumption data...");

            var consumptions = await _electricityConsumptionService.AggregateConsumptionDataAsync();

            _logger.LogInformation("Electricity consumption data aggregated and added to database successfully.");

            return CreatedAtAction(nameof(AggregateConsumptionData), consumptions);
        }
    }
}
