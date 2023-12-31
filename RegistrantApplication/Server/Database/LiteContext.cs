﻿using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Shared.Database.Accounts;
using RegistrantApplication.Shared.Database.Admin;
using RegistrantApplication.Shared.Database.Contragents;
using RegistrantApplication.Shared.Database.Orders;

namespace RegistrantApplication.Server.Database
{
    public sealed class LiteContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSession> AccountsSessions { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Auto> AccountsAutos { get; set; }
        public DbSet<Document> AccountsDocuments { get; set; }
        public DbSet<Contragent> Contragents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<FileDocument> Files { get; set; }
        public DbSet<Event> Events { get; set; }

        public LiteContext()
        {
            Database.MigrateAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigSrv.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.SetNull;
            }

            modelBuilder.Entity<AccountRole>().HasData(
                new AccountRole() { IdRole = 1, Title = "Гость", CanLogin = true, IsDefault = true }
            );
        }
        

    }
}
