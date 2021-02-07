using DakarRally.Repository.Interfaces;
using DakarRally.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DakarRally.Repository.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccess(this IServiceCollection service)
        {
            service.AddTransient<IRaceRepository, RaceRepository>();
            service.AddTransient<IVehicleRepository, VehicleRepository>();
        }
    }
}
