using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoscowWeather.DbModels;
using MoscowWeather.Models;

namespace MoscowWeather.Controllers
{
    public class HomeController : Controller
    {
        private MoscowWeatherContext _dbConnection;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MoscowWeatherContext context)
        {
            _logger = logger;
            _dbConnection = context;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public async Task<IActionResult> ShowWeatherArchives(
            Months month,
            int? year,
            int page = 1)
        {
            int pageSize = 10;

            var source = _dbConnection.Weather.Select(x => x);

            if (month == 0 && year != null)
            {
                source = 
                    source.Where(
                        obj => 
                            obj.Date.Year.Equals(
                                year));
            }
            if (month > 0 && year != null)
            {
                source = 
                    source.Where(obj => 
                        obj.Date.Year.Equals(year) && 
                        obj.Date.Month.Equals((int)month));
            }

            return 
                View(
                    await PageViewModel.CreateAsync(
                        source.AsNoTracking(), 
                        page, 
                        pageSize, 
                        month, 
                        year));
        }

        public IActionResult LoadNewWeatherAcrchives()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFiles(ExcelFileViewModel fileViewModel)
        {
            if (!ModelState.IsValid)
                return View("LoadNewWeatherAcrchives", fileViewModel);

            foreach (var formFile in fileViewModel.ExcelFiles)
            {
                try
                {
                    if (formFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(stream);
                            fileViewModel.ReadFromExcel(stream, _dbConnection);
                        }
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(formFile.FileName, $"{formFile.FileName} : {e.Message}" );
                }
            }

            return View("LoadNewWeatherAcrchives", fileViewModel);
        }
    }
}
