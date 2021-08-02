using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teste.Domain.Entities;

namespace Teste.Infrastructure.Data.Contexts
{
    public class TesteContext : DbContext
    {
        public TesteContext(DbContextOptions options) : base(options) { }

        public DbSet<Contato> Contato { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AddDataCriacao();
            AddDataAlteracao();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddDataCriacao() =>
            ChangeTracker.Entries().Where(p => p.Entity is Entity && p.State.Equals(EntityState.Added)).ToList()
                .ForEach(p => ((Entity)p.Entity).DataCriacao = DateTime.UtcNow);

        private void AddDataAlteracao() =>
            ChangeTracker.Entries().Where(p => p.Entity is Entity && p.State.Equals(EntityState.Modified)).ToList()
                .ForEach(p => ((Entity)p.Entity).DataAlteracao = DateTime.UtcNow);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}