using AutoMapper;
using AutoMapper.Features;
using Castle.Core.Logging;
using Electricity.Application.Services;
using Electricity.Domain.Entities;
using Electricity.Domain.Interfaces;
using Electricity.Domain.IService;
using Electricity.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electricity.Tests.Services
{
    public class ElectricityConsumptionServiceTests
    {
        private readonly Mock<ICsvDataService> _csvDataServiceMock; 
        private readonly Mock<IElectricityConsumptionRepository> _repositoryMock; 
        private readonly Mock<IMapper> _mapperMock; 
        private readonly Mock<ILogger<ElectricityConsumptionAggregate>> _loggerMock; 
        private readonly ElectricityConsumptionService _service;

        public ElectricityConsumptionServiceTests()
        { 
            _csvDataServiceMock = new Mock<ICsvDataService>();
            _repositoryMock = new Mock<IElectricityConsumptionRepository>(); 
            _mapperMock = new Mock<IMapper>(); 
            _loggerMock = new Mock<ILogger<ElectricityConsumptionAggregate>>(); 
            _service = new ElectricityConsumptionService(_csvDataServiceMock.Object,
            _mapperMock.Object, _repositoryMock.Object, _loggerMock.Object); 
        }

        [Fact]
        public async Task AggregateConsumptionDataAsync_ShouldReturnElectricityAggregates()
        { 
            // Arrange
            var electricityConsumption = new ElectricityData()
            {
                Regions = "Alytaus regiono tinklas",
                OBJ_NUMERIS = "27722",
                Timestamp = new DateTime(2022, 04, 30),
                Name = "Butas",
                Type = "Ne GV",
                PPlus = (decimal)3201.05970,
                PMinus = (decimal)0.00000
            };
            var consumptions = new List<ElectricityData>() { electricityConsumption };
            _csvDataServiceMock.Setup(mock => mock.FilterAndAggregateElectricityData()).ReturnsAsync(consumptions); 
            var expectedAggregates = new List<ElectricityConsumptionAggregate>(); 
            _mapperMock.Setup(mock => mock.Map<List<ElectricityConsumptionAggregate>>(consumptions)) .Returns(expectedAggregates);
            
            // Act
            var actualAggregates = await _service.AggregateConsumptionDataAsync(); 
            
            // Assert
            Assert.Equal(expectedAggregates, actualAggregates); }
        
        [Fact] 
        public async Task AggregateConsumptionDataAsync_ShouldSaveElectricityConsumptionAggregates() 
        {
            // Arrange
            var electricityConsumption = new ElectricityData()
            {
                Regions = "Alytaus regiono tinklas",
                OBJ_NUMERIS = "27722",
                Timestamp = new DateTime(2022, 04, 30),
                Name = "Butas",
                Type = "Ne GV",
                PPlus = (decimal)3201.05970,
                PMinus = (decimal)0.00000
            };
            var consumptions = new List<ElectricityData>() { electricityConsumption };

            _csvDataServiceMock.Setup(mock => mock.FilterAndAggregateElectricityData()).ReturnsAsync(consumptions); 
           
            // Act
            await _service.AggregateConsumptionDataAsync();
           
            // Assert
            _repositoryMock.Verify(mock => mock.SaveElectricityConsumptionAggregatesAsync(consumptions), Times.Once); 
        }

        
        [Fact]
        public async Task AggregateConsumptionDataAsync_Should_Filter_And_Aggregate_Electricity_Data_And_Save_Aggregates()
        {
           // Arrange
           var electricityConsumption = new ElectricityData()
           {
               Regions = "Alytaus regiono tinklas",
               OBJ_NUMERIS = "27722",
               Timestamp = new DateTime(2022, 04, 30),
               Name = "Butas",
               Type = "Ne GV",
               PPlus = (decimal)3201.05970,
               PMinus = (decimal)0.00000
           };
            var expectedAggregates = new List<ElectricityData> { electricityConsumption };
            _csvDataServiceMock.Setup(x => x.FilterAndAggregateElectricityData()).ReturnsAsync(expectedAggregates);

            var savedAggregates = new List<ElectricityData>();
            _repositoryMock.Setup(x => x.SaveElectricityConsumptionAggregatesAsync(It.IsAny<IEnumerable<ElectricityData>>()))
                .Callback<IEnumerable<ElectricityData>>(aggregates => savedAggregates.AddRange(aggregates))
                .Returns(Task.CompletedTask);

            //Act
           var result = await _service.AggregateConsumptionDataAsync();

            //Assert
            _csvDataServiceMock.Verify(x => x.FilterAndAggregateElectricityData(), Times.Once);
            _repositoryMock.Verify(x => x.SaveElectricityConsumptionAggregatesAsync(expectedAggregates), Times.Once);
            Assert.Equal(expectedAggregates, savedAggregates);
        }
    }
}
