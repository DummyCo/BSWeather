﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSWeather.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Cities = new HashSet<City>();
            SearchHistoryRecords = new HashSet<SearchHistoryRecord>();
        }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<SearchHistoryRecord> SearchHistoryRecords { get; set; }
    }
}
