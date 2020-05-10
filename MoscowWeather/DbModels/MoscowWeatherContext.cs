using Microsoft.EntityFrameworkCore;

namespace MoscowWeather.DbModels
{
    public partial class MoscowWeatherContext : DbContext
    {
        public MoscowWeatherContext()
        {
        }

        public MoscowWeatherContext(DbContextOptions<MoscowWeatherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Weather> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weather>(entity =>
            {
                entity.ToTable("weather");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AtmosphericPressure)
                    .HasColumnName("atmospheric_pressure")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Cloudiness)
                    .HasColumnName("cloudiness")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.H)
                    .HasColumnName("h")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Humidity)
                    .HasColumnName("humidity")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Td)
                    .HasColumnName("td")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Temperature)
                    .HasColumnName("temperature")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Vv)
                    .HasColumnName("vv")
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.WeatherCondition)
                    .HasColumnName("weather_condition")
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.WindDirection)
                    .HasColumnName("wind_direction")
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.WindSpeed)
                    .HasColumnName("wind_speed")
                    .HasColumnType("numeric(8, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
