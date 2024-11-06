using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PromoCodeFactory.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<CustomerPreference> CustomerPreferences { get; set; }

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Employee, Roles, Customer,Preference и PromoCode
//*
            //PromoCode имеет ссылку на Preference
            modelBuilder.Entity<PromoCode>()
                .HasOne<Preference>(p => p.Preference)
                .WithMany(c => c.PromoCodes);

            //и Employee имеет ссылку на Role
            modelBuilder.Entity<Employee>()
                .HasOne<Role>(e => e.Role)
                .WithMany(r => r.Employees);

            //Customer имеет набор Preference, и сущности связаны через Many - to - many,
            //нужно сделать маппинг через сущность CustomerPreference
            modelBuilder.Entity<CustomerPreference>()
                .HasKey(cp => new { cp.CustomerId, cp.PreferenceId });
            modelBuilder.Entity<CustomerPreference>()
                .HasOne<Customer>(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne<Preference>(cp => cp.Preference)
                .WithMany(p => p.CustomerPreferences);


            //Связь Customer и Promocode реализовать через One-To-Many, промокод может быть выдан только одному клиенту.
            modelBuilder.Entity<PromoCode>()
                .HasOne<Customer>( p => p.Customer)
                .WithMany(c => c.Promocodes);
// */
            //Строковые поля должны иметь ограничения на MaxLength
            int MaxLength = 100;        
            MaxLength = Math.Min(MaxLength, int.MaxValue );

            modelBuilder.Entity<Employee>().Property(c => c.FirstName).HasMaxLength(MaxLength);
            modelBuilder.Entity<Employee>().Property(c => c.LastName).HasMaxLength(MaxLength);            
            modelBuilder.Entity<Employee>().Property(c => c.Email).HasMaxLength(MaxLength);

            modelBuilder.Entity<Role>().Property(c => c.Name).HasMaxLength(MaxLength);
            modelBuilder.Entity<Role>().Property(c => c.Description).HasMaxLength(MaxLength);

            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(MaxLength);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(MaxLength);
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(MaxLength);

            modelBuilder.Entity<Preference>().Property(c => c.Name).HasMaxLength(MaxLength);

            modelBuilder.Entity<PromoCode>().Property(c => c.Code).HasMaxLength(MaxLength);
            modelBuilder.Entity<PromoCode>().Property(c => c.ServiceInfo).HasMaxLength(MaxLength);
            modelBuilder.Entity<PromoCode>().Property(c => c.PartnerName).HasMaxLength(MaxLength);
            // */
            }

    }
}
