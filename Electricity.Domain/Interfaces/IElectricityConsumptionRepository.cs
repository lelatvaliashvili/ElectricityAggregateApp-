using Electricity.Domain.Entities;
using Electricity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electricity.Domain.Interfaces
{
    public interface IElectricityConsumptionRepository
    {
        Task<IReadOnlyList<ElectricityConsumptionAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        Task SaveElectricityConsumptionAggregatesAsync(IEnumerable<ElectricityData> aggregateList);
    }
}
