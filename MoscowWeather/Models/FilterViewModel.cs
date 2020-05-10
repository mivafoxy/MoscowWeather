using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoscowWeather.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(Months month, int? year)
        {
            SelectedMonth = month;
            SelectedYear = year;
        }
        public Months SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
    }
}
