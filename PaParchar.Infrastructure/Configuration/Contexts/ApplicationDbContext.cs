using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
