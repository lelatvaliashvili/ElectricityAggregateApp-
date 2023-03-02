using Electricity.Domain.Interfaces;
using Electricity.Domain.IService;
using Electricity.Domain.Models;
using Electricity.Infrastructure.Data;
using Electricity.Domain.Models;

namespace Electricity.Application.Services
{
    public class ElectricityConsumptionService : IElectricityConsumptionService
    {
        private readonly ICsvDataService _csvDataService;
        private readonly IElectricityConsumptionRepository _repository;

        public ElectricityConsumptionService(ICsvDataService csvDataService, 
                                             IElectricityConsumptionRepository repository)
        {
            _csvDataService = csvDataService;
            _repository = repository;
        }
        public async Task<IEnumerable<ElectricityConsumptionAggregate>> AggregateConsumptionDataAsync()
        {
            var consumptions = await _csvDataService.FilterAndAggregateElectricityData();
            await SaveElectricityConsumptionAggregatesAsync(consumptions);
            return consumptions;
        }

        public async Task<IEnumerable<ElectricityConsumptionAggregate>> GetAggregateConsumptionDataAsync()
        {
            var result = await _repository.GetAllAsync();
            return result;
        }

        private async Task SaveElectricityConsumptionAggregatesAsync(IEnumerable<ElectricityConsumptionAggregate> consumptions)
        {
            await _repository.SaveElectricityConsumptionAggregatesAsync(consumptions);
        }
    }
}
