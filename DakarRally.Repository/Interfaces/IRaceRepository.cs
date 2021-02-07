using DakarRally.Repository.Models;
using DakarRally.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DakarRally.Repository.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetRacesAsync();
        Task<Race> GetRaceByIdAsync(int id);
        Task PostRaceAsync(int year);
        Task PutRaceAsync(int id, Race race);
        Task DeleteRaceAsync(int id);
        Task StartRaceAsync(int id);
        Task<RaceStatusDTO> GetRaceStatusAsync(int id);
    }
}
