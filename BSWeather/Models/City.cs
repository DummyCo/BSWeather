﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSWeather.Models
{
    public class City
    {
        public City()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        
        public int ExternalIdentifier { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
