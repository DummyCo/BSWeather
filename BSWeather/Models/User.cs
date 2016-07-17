using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSWeather.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();

        public virtual ICollection<SearchHistoryRecord> SearchHistoryRecords { get; set; } = new HashSet<SearchHistoryRecord>();
    }
}
