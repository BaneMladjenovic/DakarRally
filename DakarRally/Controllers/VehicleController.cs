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

        // GET: api/Vehicle
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.vehicleRepository.GetVehiclesAsync();
            return Ok(result);
        }

        // GET: api/Vehicle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.vehicleRepository.GetVehicleByIdAsync(id);
            return Ok(result);
        }

        // POST: api/Vehicle
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            await this.vehicleRepository.PostVehicleAsync(vehicle);
            return Ok("Vehicle created successfully!");
        }

        // PUT: api/Vehicle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            await this.vehicleRepository.PutVehicleAsync(id, vehicle);
            return Ok("Vehicle updated successfully!");
        }

        // DELETE: api/Vehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.vehicleRepository.DeleteVehicleAsync(id);
            return Ok("Vehicle deleted successfully!");
        }

        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var result = await this.vehicleRepository.GetStatisticsAsync(id);
            return Ok(result);
        }

        // POST: api/Vehicle/find
        [HttpPost("find")]
        public async Task<IActionResult> FindVehicles(VehicleFilterDTO vehicleFilter)
        {
            var result = await this.vehicleRepository.FindVehiclesAsync(vehicleFilter);
            return Ok(result);
        }

        //// GET: api/Vehicle/leaderboard
        //[HttpGet("leaderboard")]
        //public async Task<IActionResult> GetLeaderBoard()
        //{
        //    var result = await this.vehicleRepository.GetLeaderBoardAsync();
        //    return Ok(result);
        //}

        //// GET: api/Vehicle/type/leaderboard
        //[HttpGet("{type}/leaderboard")]
        //public async Task<IActionResult> GetLeaderBoardByType(string type)
        //{
        //    var result = await this.vehicleRepository.GetLeaderBoardByTypeAsync(type);
        //    return Ok(result);
        //}
    }
}
