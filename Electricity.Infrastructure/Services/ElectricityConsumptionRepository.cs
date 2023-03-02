using AutoMapper;
using Electricity.Domain.Entities;
using Electricity.Domain.Interfaces;
using Electricity.Domain.Models;
using Electricity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Electricity.Infrastructure.Services
{
    public class ElectricityConsumptionRepository : IElectricityConsumptionRepository
    {
        private readonly ElectricityDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ElectricityConsumptionRepository> _logger;

        public ElectricityConsumptionRepository(ElectricityDbContext dbContext,IMapper mapper, ILogger<ElectricityConsumptionRepository> logger) //IUnitOfWork unitOfWork, 
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IReadOnlyList<ElectricityConsumptionAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var electricityData = await _dbContext.ElectricityData.ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} electricity consumption aggregates from the database", electricityData.Count);

            var electricityAggregates = _mapper.Map<List<ElectricityConsumptionAggregate>>(electricityData);
            return electricityAggregates;
        }

        public async Task SaveElectricityConsumptionAggregatesAsync(IEnumerable<ElectricityConsumptionAggregate> aggregateList)
        {
            var electricityAggregates = _mapper.Map<List<ElectricityData>>(aggregateList);
            var distinctList = electricityAggregates
                .GroupBy(e => new { e.Regions })
                .Select(g => g.First())
                .ToList();

            foreach (var aggregate in distinctList)
            {
                var existingEntity = _dbContext.Set<ElectricityData>()
                    .FirstOrDefault(e => e.Regions == aggregate.Regions);

                if (existingEntity != null)
                {
                    _logger.LogInformation("Entity with same key exists, skip updating");
                    continue;
                }
                else
                {
                    _logger.LogInformation("Entity with same key does not exist, create new entity instance");
                    _dbContext.Set<ElectricityData>().Add(aggregate);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}