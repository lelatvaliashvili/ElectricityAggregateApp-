using CsvHelper;
using Electricity.Domain.Entities;
using Electricity.Domain.IService;
using Electricity.Infrastructure.Helpers;
using System.Globalization;
using CsvHelper.Configuration;
using Electricity.Domain.Models;
using System.Collections.Concurrent;

namespace Electricity.Infrastructure.Services
{
    public class CsvDataService : ICsvDataService
    {
        private readonly HttpClient _httpClient;

        // in a real-world application, it would be better to configure HttpClient
        // with appropriate timeout and retry policies, as well as configure it to use a custom HttpClientHandler
        public CsvDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ElectricityConsumptionAggregate>> FilterAndAggregateElectricityData()
        {
            var electricityConsumptionData = new ConcurrentQueue<ElectricityConsumptionAggregate>();
            var tasks = Constants.Urls.AsParallel().Select(async url =>
            {
                using var httpClient = new HttpClient();
                var stream = await httpClient.GetStreamAsync(url);
                using var reader = new StreamReader(stream);
                using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    HeaderValidated = null
                });

                var electricityData = csvReader.GetRecords<ElectricityData>()
                    .AsParallel()
                    .Where(x => x.Name == "Butas")
                    .GroupBy(x => new { x.Regions })
                    .Select(x => new ElectricityConsumptionAggregate
                    {
                        Region = x.Key.Regions,
                        TotalPositive = x.Sum(y => y.PPlus),
                        TotalNegative = x.Sum(y => y.PMinus),
                        TimeStamp = x.First().Timestamp,
                        Name = x.First().Name,
                        Type = x.First().Type,
                        OBJ_NUMERIS = x.First().OBJ_NUMERIS
                    }).ToList();

                foreach (var data in electricityData)
                {
                    electricityConsumptionData.Enqueue(data);
                }
            }).ToList();

            await Task.WhenAll(tasks);

            return electricityConsumptionData.Distinct().ToList();
        }
    }
}
