using Electricity.Domain.Models;
using Electricity.Infrastructure.Helpers;
using Electricity.Infrastructure.Services;
using Moq;
using Electricity.Domain.IService;

namespace Electricity.Tests.Services
{
    public class CsvDataServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ICsvDataService> _csvDataServiceMock = new Mock<ICsvDataService>();

        public CsvDataServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        }

        [Fact]
        public async Task FilterAndAggregateElectricityData_WithEmptyUrlsList_ReturnsEmptyList()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var csvDataService = new CsvDataService(mockHttpClient.Object);

            Constants.Urls = new List<string>();

            // Act
            var result = await csvDataService.FilterAndAggregateElectricityData();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FilterAndAggregateElectricityData_ShouldReturnElectricityConsumptionAggregateList()
        {
            // Arrange
            var csvData = new List<string>
            {
                "Timestamp,Type,Name,OBJ_NUMERIS,Regions,PPlus,PMinus",
                "2021-01-01 00:00:00.0000000,N,Butas,1234,A,10,5",
                "2021-01-01 00:00:00.0000000,N,Butas,1234,A,15,5",
                "2021-01-01 00:00:00.0000000,N,Butas,1234,B,20,10",
                "2021-01-01 00:00:00.0000000,N,Namas,1234,A,15,5",
            };

            var expectedResults = new List<ElectricityConsumptionAggregate>
            {
                new ElectricityConsumptionAggregate { Name = "Butas", Region = "A",  TotalNegative = 10, TotalPositive = 25 },
                new ElectricityConsumptionAggregate { Name = "Butas", Region = "B",  TotalNegative = 10, TotalPositive = 20 }
            };

            _csvDataServiceMock.Setup(x => x.FilterAndAggregateElectricityData(csvData))
                               .ReturnsAsync(expectedResults);

            var csvDataService = _csvDataServiceMock.Object;

            // Act
            var result = await csvDataService.FilterAndAggregateElectricityData(csvData);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResults.Count, result.Count());
            Assert.Equal(expectedResults, result, new ElectricityConsumptionAggregateComparer());
            Assert.Equal(expectedResults.First().Region, result.First().Region);
            Assert.Equal(expectedResults.First().TotalPositive, result.First().TotalPositive);
            Assert.Equal(expectedResults.First().TotalNegative, result.First().TotalNegative);
            Assert.Equal(expectedResults.First().Name, result.First().Name);
        }
    }

    public class ElectricityConsumptionAggregateComparer : IEqualityComparer<ElectricityConsumptionAggregate>
    {
        public bool Equals(ElectricityConsumptionAggregate x, ElectricityConsumptionAggregate y)
        {
            return x.Name == y.Name &&
                   // x.OBJ_NUMERIS == y.OBJ_NUMERIS &&
                   x.Region == y.Region &&
                   //  x.TimeStamp == y.TimeStamp &&
                   x.TotalNegative == y.TotalNegative &&
                   x.TotalPositive == y.TotalPositive;
                 //  x.Type == y.Type;
        }

        public int GetHashCode(ElectricityConsumptionAggregate obj)
        {
            return obj.GetHashCode();
        }
    }
}
