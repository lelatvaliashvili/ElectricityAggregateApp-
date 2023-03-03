using CsvHelper.Configuration.Attributes;

namespace Electricity.Domain.Models
{
    public class ElectricityConsumptionAggregate
    {
        [Name("TINKLAS")]
        public string Region { get; set; }

        [Name("P+")]
        [Default(0)]
        public decimal TotalPositive { get; set; }

        [Name("P-")]
        [Default(0)]
        public decimal TotalNegative { get; set; }

        [Name("OBT_PAVADINIMAS")]
        public string Name { get; set; }
    }
}
