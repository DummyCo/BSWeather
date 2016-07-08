using System.ComponentModel.DataAnnotations;

namespace BSWeather.Models
{
    public class CitySearch
    {
        [Required(ErrorMessage = "Укажите, пожалуйста, название")]
        public string CityName { get; set; }
        
        [Range(1, 7, ErrorMessage = "Прогноз вне диапазаона")]
        public int Days { get; set; }
    }
}
