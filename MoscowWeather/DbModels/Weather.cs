using System;

namespace MoscowWeather.DbModels
{
    public partial class Weather
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? Td { get; set; }
        public decimal? AtmosphericPressure { get; set; }
        public string WindDirection { get; set; }
        public decimal? WindSpeed { get; set; }
        public decimal? Cloudiness { get; set; }
        public decimal? H { get; set; }
        public string Vv { get; set; }
        public string WeatherCondition { get; set; }
        public TimeSpan Time { get; set; }
    }
}
