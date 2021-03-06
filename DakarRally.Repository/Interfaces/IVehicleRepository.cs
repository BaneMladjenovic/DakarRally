﻿using DakarRally.Repository.Models;
using DakarRally.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DakarRally.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task<Vehicle> PostVehicleAsync(Vehicle vehicle);
        Task<Vehicle> PutVehicleAsync(int id, Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int id);
        Task<IEnumerable<VehicleStatistic>> GetLeaderboardAsync();
        Task<IEnumerable<VehicleStatistic>> GetLeaderboardByVehicleTypeAsync(string type);
        Task<IEnumerable<VehicleStatistic>> GetStatisticsAsync(int id);
        Task<IEnumerable<Vehicle>> FindVehiclesAsync(VehicleFilterDTO vehicleFilter);
    }
}
