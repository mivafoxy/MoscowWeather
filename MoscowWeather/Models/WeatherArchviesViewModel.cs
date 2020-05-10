using Microsoft.AspNetCore.Mvc.Rendering;
using MoscowWeather.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoscowWeather.Models
{
    public enum Months
    {
        [Display(Name = "Ничего")]
        Nothing = 0,
        [Display(Name = "Январь")]
        January = 1,
        [Display(Name = "Февраль")]
        February = 2,
        [Display(Name = "Март")]
        March = 3,
        [Display(Name = "Апрель")]
        April = 4,
        [Display(Name = "Май")]
        May = 5,
        [Display(Name = "Июнь")]
        June = 6,
        [Display(Name = "Июль")]
        July = 7,
        [Display(Name = "Август")]
        August = 8,
        [Display(Name = "Сентябрь")]
        September = 9,
        [Display(Name = "Октябрь")]
        October = 10,
        [Display(Name = "Ноябрь")]
        November = 11,
        [Display(Name = "Декабрь")]
        December = 12
    }

    public class WeatherArchviesViewModel
    {
        public IEnumerable<Weather> WeatherModels { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
