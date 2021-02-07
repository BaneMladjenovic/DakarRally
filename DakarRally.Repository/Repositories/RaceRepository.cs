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
using System.Threading;
using System.Threading.Tasks;

namespace DakarRally.Repository.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        public async Task<IEnumerable<Race>> GetRacesAsync()
        {
            using(var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.Race.Include(race => race.Vehicles).ToListAsync();
            }
        }

        public async Task<Race> GetRaceByIdAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                return await context.Race.Where(x => x.Id == id).Include(race => race.Vehicles).FirstOrDefaultAsync();
            }
        }

        public async Task PostRaceAsync(int year)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                Race race = new Race() { Distance = 10000, Status = "Pending", Year = year }; 
                await context.AddAsync(race);
                await context.SaveChangesAsync();
            }
        }

        public async Task PutRaceAsync(int id, Race race)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                var data = await context.Race.Where(x => x.Id == id).FirstOrDefaultAsync();
                data.Distance = race.Distance;
                data.Status = race.Status;
                data.Year = race.Year;
                data.Vehicles = race.Vehicles;

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteRaceAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                var data = await context.Race.Where(x => x.Id == id).FirstOrDefaultAsync();

                context.Race.Remove(data);

                await context.SaveChangesAsync();
            }
        }

        public async Task StartRaceAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                if (!context.Race.Where(x => x.Id == id).Any(x => x.Status == RaceStatus.Running.ToString()))
                {
                    var data = await context.Race.Where(x => x.Id == id).Include(race => race.Vehicles).FirstOrDefaultAsync();
                    data.Status = RaceStatus.Running.ToString();
                    await context.SaveChangesAsync();

                    List<Thread> threads = new List<Thread>();
                    foreach (var vehicle in data.Vehicles)
                    {
                        Thread participant = new Thread(StartRace);
                        threads.Add(participant);
                        participant.Start(vehicle);
                        participant.Join();
                    }

                    data.Status = RaceStatus.Finished.ToString();
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<RaceStatusDTO> GetRaceStatusAsync(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                var race = await context.Race.Where(x => x.Id == id).Include(race => race.Vehicles).FirstOrDefaultAsync();
                var statusCount =  await context.VehicleStatistic.Where(x => race.Vehicles.Select(z => z.Id).Contains(x.VehicleId)).GroupBy(y => y.Status).Select(z => new { Status = z.Key, Count = z.Count() }).ToListAsync();
                var typeCount = race.Vehicles.GroupBy(x => x.Type).Select(z => new { Type = z.Key, Count = z.Count() }).ToList();

                var raceStatus = new RaceStatusDTO() { Status = race.Status, VehiclesGroupeByStatus = new List<string>(), VehiclesGroupeByVehicleType = new List<string>() };

                foreach (var item in statusCount)
                {
                    raceStatus.VehiclesGroupeByStatus.Add($"{item.Status}: {item.Count}");
                }

                foreach (var item in typeCount)
                {
                    raceStatus.VehiclesGroupeByVehicleType.Add($"{item.Type}: {item.Count}");
                }

                return raceStatus;
            }
        }

        public static void StartRace(object vehicle)
        {
            using (var context = new ApplicationDBContext())
            {
                Random rnd = new Random();
                var estimatedSeconds = Math.Round(10000 / (decimal)(vehicle as Vehicle).Speed) * 3600;

                VehicleStatistic vehicleStatistic = new VehicleStatistic()
                {
                    Distance = 0,
                    Status = "Active",
                    FinishTimeInHours = 0,
                    VehicleId = (vehicle as Vehicle).Id
                };


                MalfunctionStatistic malfunctionStatistic = new MalfunctionStatistic()
                {
                    NumberOfLightMalfunctions = 0,
                    NumberOfHeavyMalfunctions = 0
                };

                for (int i = 1; i < estimatedSeconds; i++)
                {
                    if (i % 3600 == 0)
                    {
                        var check = rnd.Next(101);
                        switch ((vehicle as Vehicle).Type)
                        {
                            case nameof(VehicleType.Car):
                                switch ((vehicle as Vehicle).SubType)
                                {
                                    case nameof(CarType.Sport):
                                        if (check <= 2)
                                        {
                                            malfunctionStatistic.NumberOfHeavyMalfunctions += 1;
                                            vehicleStatistic.Status = "Inactive";
                                            vehicleStatistic.Distance = i / 3600 * (vehicle as Vehicle).Speed;
                                            //vehicleStatistic.FinishTime = vehicleStatistic.FinishTime.AddSeconds(i);

                                            context.VehicleStatistic.Add(vehicleStatistic);
                                            context.SaveChanges();

                                            malfunctionStatistic.VehicleStatisticId = vehicleStatistic.Id;
                                            context.MalfunctionStatistic.Add(malfunctionStatistic);
                                            context.SaveChanges();

                                            return;
                                        }

                                        if (check <= 12)
                                        {
                                            malfunctionStatistic.NumberOfLightMalfunctions += 1;
                                            estimatedSeconds += 5 * 3600;
                                        }
                                        break;
                                    case nameof(CarType.Terrain):
                                        if (check <= 1)
                                        {
                                            malfunctionStatistic.NumberOfHeavyMalfunctions += 1;
                                            vehicleStatistic.Status = "Inactive";
                                            vehicleStatistic.Distance = i / 3600 * (vehicle as Vehicle).Speed;
                                            //vehicleStatistic.FinishTime = vehicleStatistic.FinishTime.AddSeconds(i);

                                            context.VehicleStatistic.Add(vehicleStatistic);
                                            context.SaveChanges();

                                            malfunctionStatistic.VehicleStatisticId = vehicleStatistic.Id;
                                            context.MalfunctionStatistic.Add(malfunctionStatistic);
                                            context.SaveChanges();

                                            return;
                                        }

                                        if (check <= 3)
                                        {
                                            malfunctionStatistic.NumberOfLightMalfunctions += 1;
                                            estimatedSeconds += 5 * 3600;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case nameof(VehicleType.Motorcycle):
                                switch ((vehicle as Vehicle).SubType)
                                {
                                    case nameof(MotorcycleType.Cross):
                                        if (check <= 2)
                                        {
                                            malfunctionStatistic.NumberOfHeavyMalfunctions += 1;
                                            vehicleStatistic.Status = "Inactive";
                                            vehicleStatistic.Distance = i / 3600 * (vehicle as Vehicle).Speed;
                                            //vehicleStatistic.FinishTime = vehicleStatistic.FinishTime.AddSeconds(i);

                                            context.VehicleStatistic.Add(vehicleStatistic);
                                            context.SaveChanges();

                                            malfunctionStatistic.VehicleStatisticId = vehicleStatistic.Id;
                                            context.MalfunctionStatistic.Add(malfunctionStatistic);
                                            context.SaveChanges();

                                            return;
                                        }

                                        if (check <= 3)
                                        {
                                            malfunctionStatistic.NumberOfLightMalfunctions += 1;
                                            estimatedSeconds += 3 * 3600;
                                        }
                                        break;
                                    case nameof(MotorcycleType.Sport):
                                        if (check <= 10)
                                        {
                                            malfunctionStatistic.NumberOfHeavyMalfunctions += 1;
                                            vehicleStatistic.Status = "Inactive";
                                            vehicleStatistic.Distance = i / 3600 * (vehicle as Vehicle).Speed;
                                            //vehicleStatistic.FinishTime = vehicleStatistic.FinishTime.AddSeconds(i);

                                            context.VehicleStatistic.Add(vehicleStatistic);
                                            context.SaveChanges();

                                            malfunctionStatistic.VehicleStatisticId = vehicleStatistic.Id;
                                            context.MalfunctionStatistic.Add(malfunctionStatistic);
                                            context.SaveChanges();

                                            return;
                                        }

                                        if (check <= 18)
                                        {
                                            malfunctionStatistic.NumberOfLightMalfunctions += 1;
                                            estimatedSeconds += 3 * 3600;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case nameof(VehicleType.Truck):
                                if (check <= 4)
                                {
                                    malfunctionStatistic.NumberOfHeavyMalfunctions += 1;
                                    vehicleStatistic.Status = "Inactive";
                                    vehicleStatistic.Distance = i / 3600 * (vehicle as Vehicle).Speed;
                                    //vehicleStatistic.FinishTime = vehicleStatistic.FinishTime.AddSeconds(i);

                                    context.VehicleStatistic.Add(vehicleStatistic);
                                    context.SaveChanges();

                                    malfunctionStatistic.VehicleStatisticId = vehicleStatistic.Id;
                                    context.MalfunctionStatistic.Add(malfunctionStatistic);
                                    context.SaveChanges();

                                    return;
                                }

                                if (check <= 6)
                                {
                                    malfunctionStatistic.NumberOfLightMalfunctions += 1;
                                    estimatedSeconds += 7 * 3600;
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }

                vehicleStatistic.Distance = 10000;
                vehicleStatistic.FinishTimeInHours = (int)estimatedSeconds / 3600;

                context.VehicleStatistic.Add(vehicleStatistic);
                context.SaveChanges();

                malfunctionStatistic.VehicleStatisticId = vehicleStatistic.Id;
                context.MalfunctionStatistic.Add(malfunctionStatistic);
                context.SaveChanges();
            }
        }
    }
}
