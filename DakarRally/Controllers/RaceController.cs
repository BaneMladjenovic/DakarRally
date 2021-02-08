using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DakarRally.Repository.Interfaces;
using DakarRally.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DakarRally.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private IRaceRepository raceRepository;
        
        public RaceController(IRaceRepository _raceRepository)
        {
            raceRepository = _raceRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.raceRepository.GetRacesAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.raceRepository.GetRaceByIdAsync(id);
            if (result == null)
            {
                return NotFound("Race not found");
            }

            return Ok(result);
        }

        // Task Requirement 1)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] int year)
        {
            await this.raceRepository.PostRaceAsync(year);
            return Ok("Race created successfully!");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Race race)
        {
            await this.raceRepository.PutRaceAsync(id, race);
            return Ok("Race updated successfully!");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.raceRepository.DeleteRaceAsync(id);
            return Ok("Race deleted successfully!");
        }

        // Task Requirement 5)
        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartRace(int id)
        {
            var result =  await this.raceRepository.StartRaceAsync(id);
            if (result == false)
            {
                return NotFound("Couldn't start the Race because it there exist another one in status Running or the current one has finished.");
            }
            return Ok("Race started successfully!");
        }

        // Task Requirement 10)
        [HttpGet("{id}/status")]
        public async Task<IActionResult> RaceStatus(int id)
        {
            var result  = await this.raceRepository.GetRaceStatusAsync(id);
            return Ok(result);
        }
    }
}
