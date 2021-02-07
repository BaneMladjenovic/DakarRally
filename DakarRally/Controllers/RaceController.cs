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

        // GET: api/Race
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.raceRepository.GetRacesAsync();
            return Ok(result);
        }

        // GET: api/Race/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.raceRepository.GetRaceByIdAsync(id);
            return Ok(result);
        }

        // POST: api/Race
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] int year)
        {
            await this.raceRepository.PostRaceAsync(year);
            return Ok("Race created successfully!");
        }

        // PUT: api/Race/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Race race)
        {
            await this.raceRepository.PutRaceAsync(id, race);
            return Ok("Race updated successfully!");
        }

        // DELETE: api/Race/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.raceRepository.DeleteRaceAsync(id);
            return Ok("Race deleted successfully!");
        }

        // PUT: Race/5/start
        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartRace(int id)
        {
            await this.raceRepository.StartRaceAsync(id);
            return Ok("Race started successfully!");
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> RaceStatus(int id)
        {
            var result  = await this.raceRepository.GetRaceStatusAsync(id);
            return Ok(result);
        }
    }
}
