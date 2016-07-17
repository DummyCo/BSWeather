using System.Collections.Generic;

namespace BSWeather.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int ExternalIdentifier { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
