using DakarRally.Repository.Models;
using DakarRally.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DakarRally.Repository.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() : base() { }

        public DbSet<Race> Race { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehicleStatistic> VehicleStatistic { get; set; }
        public DbSet<MalfunctionStatistic> MalfunctionStatistic { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=rally.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Race>().HasData(
                 new Race() { Id = 1, Distance = 10000, Status = RaceStatus.Pending.ToString(), Year = 2012 }
            );
            modelBuilder.Entity<Vehicle>().HasData(
               new Vehicle() { Id = 1, ManufacturingDate = DateTime.UtcNow, VehicleModel = "X-RAID MINI JCW TEAM", TeamName = "(FRA)STÉPHANE PETERHANSEL", Type = VehicleType.Car.ToString(), SubType = CarType.Sport.ToString(), Speed = 140, RaceId = 1 },
               new Vehicle() { Id = 2, ManufacturingDate = DateTime.UtcNow, VehicleModel = "TOYOTA GAZOO RACING	", TeamName = "(QAT) NASSER AL-ATTIYAH", Type = VehicleType.Car.ToString(), SubType = CarType.Terrain.ToString(), Speed = 100, RaceId = 1 },
               new Vehicle() { Id = 3, ManufacturingDate = DateTime.UtcNow, VehicleModel = "KAMAZ - MASTER", TeamName = "(RUS) DMITRY SOTNIKOV", Type = "Truck", Speed = 80, RaceId = 1 },
               new Vehicle() { Id = 4, ManufacturingDate = DateTime.UtcNow, VehicleModel = "MONSTER ENERGY HONDA TEAM 2021", TeamName = "(ARG) KEVIN BENAVIDES", Type = VehicleType.Motorcycle.ToString(), SubType = MotorcycleType.Cross.ToString(), Speed = 85, RaceId = 1 },
               new Vehicle() { Id = 5, ManufacturingDate = DateTime.UtcNow, VehicleModel = "RED BULL KTM FACTORY TEAM", TeamName = "(GBR) SAM SUNDERLAND", Type = VehicleType.Motorcycle.ToString(), SubType = MotorcycleType.Sport.ToString(), Speed = 130, RaceId = 1 }
            );
        }
    }
}
