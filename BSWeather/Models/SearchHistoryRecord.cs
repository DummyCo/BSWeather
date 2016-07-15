using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSWeather.Models
{
    public class SearchHistoryRecord
    {
        public int Id { get; set; }

        public string CityName { get; set; }

        public DateTime DateTime { get; set; }
    }
}
