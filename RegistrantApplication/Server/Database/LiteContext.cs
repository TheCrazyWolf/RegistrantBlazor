using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Shared.Accounts;
using RegistrantApplication.Shared.Contragents;
using RegistrantApplication.Shared.Drivers;
using RegistrantApplication.Shared.Orders;

namespace RegistrantApplication.Server.Database
{
    public class LiteContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> AccountsSessions { get; set; }
        public DbSet<Auto> Autos { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Contragent> Contragents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        //public DbSet<Event> Events { get; set; }

        public LiteContext()
        {
            Database.MigrateAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=localDatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
