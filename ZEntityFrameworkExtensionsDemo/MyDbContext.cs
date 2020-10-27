using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZEntityFrameworkExtensionsDemo.Models;
using ZEntityFrameworkExtensionsDemo.Models.Interfaces;

namespace ZEntityFrameworkExtensionsDemo
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public DbSet<Chicken> Chickens { get; set; }
        public DbSet<ChickenBreed> ChickenBreeds { get; set; }
        public DbSet<ChickenCoop> ChickenCoops { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder
            ////Enable DebugLoggerProvider to view generated queries and other EF output.
            ////Disabling for now because the logging slows things down, a lot.
            //.UseLoggerFactory(new LoggerFactory(new[]
            //{
            //    new DebugLoggerProvider()
            //}))
            .UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=ChickenCoop;Trusted_Connection=True;")
            .UseLazyLoadingProxies()
            ;
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //_ = modelBuilder.Entity<Chicken>()
            //    .HasOne(c => c.ChickenBreed)
            //    .WithOne()
            //    .IsRequired(true);

            //_ = modelBuilder.Entity<Chicken>()
            //    .HasOne(c => c.ChickenCoop)
            //    .WithOne()
            //    .IsRequired(true);

            //_ = modelBuilder.Entity<Chicken>()
            //    .HasOne(c => c.Owner)
            //    .WithOne()
            //    .IsRequired(true);

            //_ = modelBuilder.Entity<ChickenCoop>()
            //    .HasOne(c => c.Owner)
            //    .WithOne()
            //    .IsRequired(true);

            //_ = modelBuilder.Entity<ChickenCoop>()
            //    .HasMany(c => c.HousedChickens)
            //    .WithOne()
            //    .IsRequired(true);

            //_ = modelBuilder.Entity<Owner>()
            //    .HasMany(o => o.ChickenCoops)
            //    .WithOne()
            //    .IsRequired(false);

            //_ = modelBuilder.Entity<Owner>()
            //    .HasMany(o => o.Chickens)
            //    .WithOne()
            //    .IsRequired(true);
        }

        private void OnSaveChanges()
        {
            if (ChangeTracker.HasChanges())
            {
                foreach (var entry in ChangeTracker.Entries<ICreatedModified>().ToList())
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (entry.Entity.DateCreated == default)
                            {
                                entry.Entity.DateCreated = now;
                            }
                            if (entry.Entity.DateModified == default)
                            {
                                entry.Entity.DateModified = now;
                            }
                            break;
                        case EntityState.Modified:
                            entry.Entity.DateModified = now;
                            break;
                    }
                }
            }
        }

        [Obsolete("Use SaveAsync", error: true)]
        public override int SaveChanges()
        {
            OnSaveChanges();
            return base.SaveChanges();
        }

        [Obsolete("Use SaveAsync, unless you need the int returned from DbContext.SaveChangesAsync")]
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveAsync()
        {
            OnSaveChanges();
            _ = await base.SaveChangesAsync();
        }
    }
}
