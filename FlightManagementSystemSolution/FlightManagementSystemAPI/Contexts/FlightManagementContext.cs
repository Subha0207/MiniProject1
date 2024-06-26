﻿using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystemAPI.Contexts
{
    public class FlightManagementContext : DbContext
    {

        public FlightManagementContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserInfo> UserInfos { get; set; }


        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Cancellation> Cancellations { get; set; }

        public DbSet<Refund> Refunds { get; set; }

        public DbSet<FlightRoute> Routes { get; set; }
        public DbSet<SubRoute> SubRoutes { get; set; }
        public DbSet<Flight> Flights { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            modelBuilder.Entity<Payment>().HasKey(f => f.PaymentId);

            modelBuilder.Entity<Flight>().HasKey(f => f.FlightId);

            modelBuilder.Entity<FlightRoute>().HasKey(r => r.RouteId);

            modelBuilder.Entity<SubRoute>().HasKey(s => s.SubRouteId);
            modelBuilder.Entity<Booking>().HasKey(b => b.BookingId);
            modelBuilder.Entity<Refund>().HasKey(rf => rf.RefundId);

            modelBuilder.Entity<Cancellation>().HasKey(c => c.CancellationId);

               modelBuilder.Entity<Booking>()
               .HasOne(b => b.Flight)
               .WithMany(f => f.Bookings)
               .HasForeignKey(b => b.FlightId)
               .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Booking>()
            //.HasOne(b => b.User)
            //.WithMany(u => u.Bookings)
            //.HasForeignKey(b => b.UserId)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.FlightRoute)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cancellation>()
            .HasOne(c => c.Booking)
            .WithMany(b => b.Cancellations)
            .HasForeignKey(c => c.BookingId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Cancellation>()
           .HasOne(c => c.Payment)
           .WithMany(p => p.Cancellations)
           .HasForeignKey(c => c.PaymentId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Refund>()
            .HasOne(rf => rf.Cancellation)
            .WithMany(c => c.Refunds)
            .HasForeignKey(rf => rf.CancellationId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
            .HasOne(b=> b.Booking)
            .WithMany(p=>p.Payments)
            .HasForeignKey(b => b.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FlightRoute>()
            .HasOne(r => r.Flight)
            .WithMany(f => f.Routes)
            .HasForeignKey(r => r.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubRoute>()
            .HasOne(s => s.Flight)
            .WithMany(f => f.SubRoutes)
            .HasForeignKey(s => s.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubRoute>()
            .HasOne(c => c.FlightRoute)
            .WithMany(rf => rf.SubRoutes)
            .HasForeignKey(s => s.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}