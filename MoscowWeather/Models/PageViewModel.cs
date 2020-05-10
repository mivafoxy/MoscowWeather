using Microsoft.EntityFrameworkCore;
using MoscowWeather.DbModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoscowWeather.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
        
        public static async Task<WeatherArchviesViewModel> CreateAsync(
            IQueryable<Weather> source, 
            int page, 
            int pageSize,
            Months month,
            int? year)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new WeatherArchviesViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                WeatherModels = items,
                FilterViewModel = new FilterViewModel(month, year)
            };


            return viewModel;
        }
    }
}
