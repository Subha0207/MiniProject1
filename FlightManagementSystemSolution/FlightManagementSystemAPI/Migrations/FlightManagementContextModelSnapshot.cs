﻿// <auto-generated />
using System;
using FlightManagementSystemAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightManagementSystemAPI.Migrations
{
    [DbContext(typeof(FlightManagementContext))]
    partial class FlightManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"), 1L, 1);

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<int>("NoOfPersons")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<float>("TotalAmount")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BookingId");

                    b.HasIndex("FlightId");

                    b.HasIndex("RouteId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Cancellation", b =>
                {
                    b.Property<int>("CancellationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CancellationId"), 1L, 1);

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CancellationId");

                    b.HasIndex("BookingId");

                    b.HasIndex("PaymentId");

                    b.ToTable("Cancellations");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Flight", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightId"), 1L, 1);

                    b.Property<string>("FlightName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatCapacity")
                        .HasColumnType("int");

                    b.HasKey("FlightId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.FlightRoute", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RouteId"), 1L, 1);

                    b.Property<DateTime>("ArrivalDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ArrivalLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<int>("NoOfStops")
                        .HasColumnType("int");

                    b.Property<float>("PricePerPerson")
                        .HasColumnType("real");

                    b.Property<int>("SeatsAvailable")
                        .HasColumnType("int");

                    b.HasKey("RouteId");

                    b.HasIndex("FlightId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"), 1L, 1);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.HasIndex("BookingId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Refund", b =>
                {
                    b.Property<int>("RefundId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RefundId"), 1L, 1);

                    b.Property<int>("CancellationId")
                        .HasColumnType("int");

                    b.Property<int?>("PaymentId")
                        .HasColumnType("int");

                    b.Property<string>("RefundStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RefundId");

                    b.HasIndex("CancellationId");

                    b.HasIndex("PaymentId");

                    b.ToTable("Refunds");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.SubRoute", b =>
                {
                    b.Property<int>("SubRouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubRouteId"), 1L, 1);

                    b.Property<DateTime>("ArrivalDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ArrivalLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("SubFlightId")
                        .HasColumnType("int");

                    b.HasKey("SubRouteId");

                    b.HasIndex("FlightId");

                    b.HasIndex("RouteId");

                    b.ToTable("SubRoutes");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.UserInfo", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Booking", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.Flight", "Flight")
                        .WithMany("Bookings")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlightManagementSystemAPI.Model.FlightRoute", "FlightRoute")
                        .WithMany("Bookings")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlightManagementSystemAPI.Model.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("FlightRoute");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Cancellation", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.Booking", "Booking")
                        .WithMany("Cancellations")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlightManagementSystemAPI.Model.Payment", "Payment")
                        .WithMany("Cancellations")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.FlightRoute", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.Flight", "Flight")
                        .WithMany("Routes")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Flight");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Payment", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.Booking", "Booking")
                        .WithMany("Payments")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Refund", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.Cancellation", "Cancellation")
                        .WithMany("Refunds")
                        .HasForeignKey("CancellationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlightManagementSystemAPI.Model.Payment", null)
                        .WithMany("Refunds")
                        .HasForeignKey("PaymentId");

                    b.Navigation("Cancellation");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.SubRoute", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.Flight", "Flight")
                        .WithMany("SubRoutes")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlightManagementSystemAPI.Model.FlightRoute", "FlightRoute")
                        .WithMany("SubRoutes")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("FlightRoute");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.UserInfo", b =>
                {
                    b.HasOne("FlightManagementSystemAPI.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Booking", b =>
                {
                    b.Navigation("Cancellations");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Cancellation", b =>
                {
                    b.Navigation("Refunds");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Flight", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Routes");

                    b.Navigation("SubRoutes");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.FlightRoute", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("SubRoutes");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.Payment", b =>
                {
                    b.Navigation("Cancellations");

                    b.Navigation("Refunds");
                });

            modelBuilder.Entity("FlightManagementSystemAPI.Model.User", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
