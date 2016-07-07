using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSWeather.Models
{
    public class CitySearch
    {
        [Required(ErrorMessage = "Укажите, пожалуйста, имя")]
        public string CityName { get; set; }
    }
}
