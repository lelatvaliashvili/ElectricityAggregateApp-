using Electricity.Domain.Models;

namespace Electricity.Domain.IService
{
    public interface IElectricityConsumptionService
    {
        Task<IEnumerable<ElectricityConsumptionAggregate>> AggregateConsumptionDataAsync();

        Task<IEnumerable<ElectricityConsumptionAggregate>> GetAggregateConsumptionDataAsync();
    }
}

