using DakarRally.Repository.DAL;
using DakarRally.Repository.Interfaces;
using DakarRally.Repository.Models;
using DakarRally.Shared.DTO;
using DakarRally.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DakarRally.Repository.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.Vehicle.ToListAsync();
            }
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.Vehicle.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task PostVehicleAsync(Vehicle vehicle)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                if (context.Race.Where(x => x.Id == vehicle.RaceId).FirstOrDefault().Status == RaceStatus.Pending.ToString())
                {
                    switch (vehicle.Type)
                    {
                        case nameof(VehicleType.Car):
                            switch (vehicle.SubType)
                            {
                                case nameof(CarType.Sport):
                                    vehicle.Speed = 140;
                                    break;
                                case nameof(CarType.Terrain):
                                    vehicle.Speed = 100;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case nameof(VehicleType.Motorcycle):
                            switch (vehicle.SubType)
                            {
                                case nameof(MotorcycleType.Cross):
                                    vehicle.Speed = 85;
                                    break;
                                case nameof(MotorcycleType.Sport):
                                    vehicle.Speed = 130;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case nameof(VehicleType.Truck):
                            vehicle.Speed = 80;
                            break;
                        default:
                            break;
                    }

                    await context.Vehicle.AddAsync(vehicle);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task PutVehicleAsync(int id, Vehicle vehicle)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                var data = await context.Vehicle.Where(x => x.Id == id).FirstOrDefaultAsync();
                data.TeamName = vehicle.TeamName;
                data.ManufacturingDate = vehicle.ManufacturingDate;
                data.VehicleModel = vehicle.VehicleModel;
                data.Speed = vehicle.Speed;
                data.Type = vehicle.Type;
                data.SubType = vehicle.SubType;

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteVehicleAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                var data = await context.Vehicle.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (context.Race.Where(x => x.Id == data.RaceId).FirstOrDefault().Status == RaceStatus.Pending.ToString())
                {
                    context.Vehicle.Remove(data);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<VehicleStatistic>> GetLeaderboardAsync()
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.VehicleStatistic.Include(x => x.Vehicle)
                    .OrderByDescending(x => x.Distance)
                    .ThenBy(x => x.FinishTimeInHours)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<VehicleStatistic>> GetLeaderboardByVehicleTypeAsync(string type)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.VehicleStatistic.Include(x => x.Vehicle)
                    .Where(x => x.Vehicle.Type.ToLower() == type.ToLower())
                    .OrderByDescending(x => x.Distance)
                    .ThenBy(x => x.FinishTimeInHours)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<VehicleStatistic>> GetStatisticsAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.VehicleStatistic.Include(x => x.MalfunctionStatistic).Where(x => x.VehicleId == id).ToListAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> FindVehiclesAsync(VehicleFilterDTO vehicleFilter)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                var vehicles = new List<Vehicle>();
                var statistics = context.VehicleStatistic.Include(x => x.Vehicle).AsNoTracking();

                if (!string.IsNullOrEmpty(vehicleFilter.TeamName))
                {
                    statistics = statistics.Where(x => x.Vehicle.TeamName == vehicleFilter.TeamName);
                }

                if (!string.IsNullOrEmpty(vehicleFilter.VehicleModel))
                {
                    statistics = statistics.Where(x => x.Vehicle.VehicleModel == vehicleFilter.VehicleModel);
                }

                if (vehicleFilter.ManufacturingDate.HasValue)
                {
                    statistics = statistics.Where(x => x.Vehicle.ManufacturingDate == vehicleFilter.ManufacturingDate.Value);
                }

                if (!string.IsNullOrEmpty(vehicleFilter.Status))
                {
                    statistics = statistics.Where(x => x.Status == vehicleFilter.Status);
                }

                foreach (var item in await statistics.ToListAsync())
                {
                    vehicles.Add(item.Vehicle);
                }

                if (vehicleFilter.SortOrder.HasValue && vehicleFilter.SortOrder == (int)SortOrder.ASC)
                {
                    vehicles = vehicles.OrderBy(x => x.Id).ToList();
                }
                else if (vehicleFilter.SortOrder.HasValue && vehicleFilter.SortOrder == (int)SortOrder.DESC)
                {
                    vehicles = vehicles.OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    vehicles = vehicles.OrderBy(x => x.Id).ToList();
                }

                return vehicles;
            }
        }
    }
}
