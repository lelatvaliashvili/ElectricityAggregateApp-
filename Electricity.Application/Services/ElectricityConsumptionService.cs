using Electricity.Domain.Interfaces;
using Electricity.Domain.IService;
using Electricity.Domain.Models;
using Electricity.Infrastructure.Data;
using Electricity.Domain.Models;
using Microsoft.Extensions.Logging;
using Electricity.Domain.Entities;
using AutoMapper;

namespace Electricity.Application.Services
{
    public class ElectricityConsumptionService : IElectricityConsumptionService
    {
        private readonly ICsvDataService _csvDataService;
        private readonly IElectricityConsumptionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ElectricityConsumptionAggregate> _logger;

        public ElectricityConsumptionService(ICsvDataService csvDataService, IMapper mapper,
                                             IElectricityConsumptionRepository repository,
                                             ILogger<ElectricityConsumptionAggregate> logger)
        {
            _csvDataService = csvDataService;
            _repository = repository;
            _mapper = mapper;
            _logger = logger; 

        }
        public async Task<IEnumerable<ElectricityConsumptionAggregate>> AggregateConsumptionDataAsync()
        {
            var consumptions = await _csvDataService.FilterAndAggregateElectricityData();
            
            await SaveElectricityConsumptionAggregatesAsync(consumptions);

            _logger.LogInformation("Aggregated data saved to the database");

            var electricityAggregates = _mapper.Map<List<ElectricityConsumptionAggregate>>(consumptions);

            _logger.LogInformation("Aggregated data mapped to ElectricityConsumptionAggregate objects");

            return electricityAggregates;
        }

        public async Task<IEnumerable<ElectricityConsumptionAggregate>> GetAggregateConsumptionDataAsync()
        {
            var result = await _repository.GetAllAsync();

            _logger.LogInformation("Aggregated data retrieved and mapped to ElectricityConsumptionAggregate  objects");
            return result;
        }

        private async Task SaveElectricityConsumptionAggregatesAsync(IEnumerable<ElectricityData> consumptions)
        {
            await _repository.SaveElectricityConsumptionAggregatesAsync(consumptions);
        }
    }
}
