using easyPC.Models;
using easyPC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace easyPC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Componente> Componentes { get; set; }
        public DbSet<Encomenda> Encomendas { get; set; }
        public DbSet<EncomendaComponente> EncomendaComponentes { get; set; }
        public DbSet<Montagem> Montagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EncomendaComponente>()
                .HasOne(ec => ec.Encomenda)
                .WithMany(e => e.EncomendaComponentes)
                .HasForeignKey(ec => ec.EncomendaId);

            modelBuilder.Entity<EncomendaComponente>()
                .HasOne(ec => ec.Componente)
                .WithMany(c => c.EncomendaComponentes)
                .HasForeignKey(ec => ec.ComponenteId);

            modelBuilder.Entity<Encomenda>()
                .HasOne(e => e.Montagem)
                .WithOne(m => m.Encomenda)
                .HasForeignKey<Montagem>(m => m.EncomendaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
