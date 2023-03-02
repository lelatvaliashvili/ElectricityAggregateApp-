using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Electricity.Domain.Entities
{
    public class ElectricityData
    {
        [Name("TINKLAS")]
        public string Regions { get; set; }

        [Name("PL_T")]
        public DateTime Timestamp { get; set; }

        [Name("OBT_PAVADINIMAS")]

        public string Name { get; set; }

        [Name("OBJ_GV_TIPAS")]
        public string Type { get; set; }

        [Key]
        [Name("OBJ_NUMERIS")]
        public string OBJ_NUMERIS { get; set; }

        [Name("P+")]
        [Default(0)]
        public decimal PPlus { get; set; }

        [Name("P-")]
        [Default(0)]
        public decimal PMinus { get; set; }
    }
}
