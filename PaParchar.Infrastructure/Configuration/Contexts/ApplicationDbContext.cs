using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaParchar.Domain.Entities;


namespace PaParchar.Infrastructure.Configuration.Contexts
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Parche> Parches { get; set; }
        public DbSet<ParcheHorario> ParchesHorarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura para que todas las propiedades DateTime se almacenen como UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? (v.Value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v.Value.ToUniversalTime()) : null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);

            // Conversor de TimeOnly a string (formato HH:mm)
            var timeOnlyConverter = new ValueConverter<TimeOnly, string>(
                v => v.ToString("HH:mm"),
                v => ParseTimeOnly(v));

            // Aplica los conversores a todas las propiedades 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                    else if (property.ClrType == typeof(TimeOnly))
                    {
                        property.SetValueConverter(timeOnlyConverter);
                    }
                }
            }
        }

        // Método auxiliar para parsear TimeOnly
        private static TimeOnly ParseTimeOnly(string timeString)
        {
            int hours = int.Parse(timeString.Substring(0, 2));
            int minutes = int.Parse(timeString.Substring(3, 2));
            return new TimeOnly(hours, minutes);
        }
    }
}
