using Electricity.Domain.Entities;
using Electricity.Domain.Models;

namespace Electricity.Domain.IService
{
    public interface ICsvDataService
    {
        Task<IEnumerable<ElectricityData>> FilterAndAggregateElectricityData();

        Task<IEnumerable<ElectricityConsumptionAggregate>> FilterAndAggregateElectricityData(List<string> csvData);

    }
}
