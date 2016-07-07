using System.ComponentModel.DataAnnotations;

namespace BSWeather.Models
{
    public class CitySearch
    {
        [Required(ErrorMessage = "Укажите, пожалуйста, имя")]
        public string CityName { get; set; }
    }
}
