using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DakarRally.Repository.Interfaces;
using DakarRally.Repository.Models;
using DakarRally.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DakarRally.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private IVehicleRepository vehicleRepository;

        public VehicleController(IVehicleRepository _vehicleRepository)
        {
            vehicleRepository = _vehicleRepository;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.vehicleRepository.GetVehiclesAsync();
            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.vehicleRepository.GetVehicleByIdAsync(id);
            if (result == null)
            {
                return NotFound("Vehicle not found.");
            }
            return Ok(result);
        }

        // Task Requirement 2)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            var result = await this.vehicleRepository.PostVehicleAsync(vehicle);
            if (result == null)
            {
                return NotFound("Couldn't add vehicle to Race because it is not in status pending.");
            }
            return Ok("Vehicle created successfully!");
        }

        // Task Requirement 3)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            var result = await this.vehicleRepository.PutVehicleAsync(id, vehicle);
            if (result == null)
            {
                return NotFound("Couldn't update vehicle because the Race it belongs to is not in status pending.");
            }
            return Ok("Vehicle updated successfully!");
        }

        // Task Requirement 4)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.vehicleRepository.DeleteVehicleAsync(id);
            if (result == false)
            {
                return NotFound("Couldn't remove vehicle from the Race because it is not in status pending.");
            }
            return Ok("Vehicle deleted successfully!");
        }

        // Task Requirement 6)
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var result = await this.vehicleRepository.GetLeaderboardAsync();
            return Ok(result);
        }

        // Task Requirement 7)
        [HttpGet("{type}/leaderboard")]
        public async Task<IActionResult> GetLeaderboardByVehicleType(string type)
        {
            var result = await this.vehicleRepository.GetLeaderboardByVehicleTypeAsync(type);
            return Ok(result);
        }

        // Task Requirement 8)
        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var result = await this.vehicleRepository.GetStatisticsAsync(id);
            return Ok(result);
        }

        // Task Requirement 9)
        [HttpPost("find")]
        public async Task<IActionResult> FindVehicles(VehicleFilterDTO vehicleFilter)
        {
            var result = await this.vehicleRepository.FindVehiclesAsync(vehicleFilter);
            return Ok(result);
        }
    }
}
