using Microsoft.EntityFrameworkCore;

namespace B.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        private readonly string _databaseName;

        public DataContext(string databaseName = "InMemoryDb")
        {
            _databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_databaseName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Relationships
            modelBuilder.Entity<Cliente>()
               .HasMany(c => c.Pedidos)
               .WithOne(p => p.Cliente)
               .HasForeignKey(p => p.ClienteId);
            #endregion
        }
    }
}
