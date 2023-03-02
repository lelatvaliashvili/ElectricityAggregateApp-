using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Electricity.Infrastructure.Helpers
{
    public static class Constants
    {
        public static List<string> Urls = new List<string>
        {
            "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv",
            "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv",
            "https://data.gov.lt/dataset/1975/download/10764/2022-03.csv",
            "https://data.gov.lt/dataset/1975/download/10763/2022-02.csv"
        };
    }
}
