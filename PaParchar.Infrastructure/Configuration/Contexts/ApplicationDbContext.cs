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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
