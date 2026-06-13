using Microsoft.EntityFrameworkCore;
using FinalAnalisis.Models;

namespace FinalAnalisis.Data
{
    public class FinalAnalisisContext : DbContext
    {
        public FinalAnalisisContext(DbContextOptions<FinalAnalisisContext> options) : base(options) { }

        public DbSet<Incidente> Incidentes => Set<Incidente>();
        public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Incidente>().HasKey(i => i.Id);
            modelBuilder.Entity<HistorialEstado>().HasKey(h => h.Id);
        }
    }
}