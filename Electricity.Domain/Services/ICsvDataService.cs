using Electricity.Domain.Models;

namespace Electricity.Domain.IService
{
    public interface ICsvDataService
    {
        Task<IEnumerable<ElectricityConsumptionAggregate>> FilterAndAggregateElectricityData();
    }
}
