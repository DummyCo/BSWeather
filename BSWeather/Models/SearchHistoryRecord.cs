using System;

namespace BSWeather.Models
{
    public class SearchHistoryRecord
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public virtual City City { get; set; }

        public virtual User User { get; set; }
    }
}
