using Microsoft.AspNetCore.Mvc;
using WindFarmMonitoring.Data;
using WindFarmMonitoring.Dto;
using WindFarmMonitoring.Models;

namespace WindFarmMonitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorDataController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public SensorDataController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // GET: api/SensorData
        [HttpGet]
        public async Task<List<SensorDataReadDto>> Get([FromQuery] string? sortBy)
        {
            var data = await _mongoDBService.GetSensorDataAsync(sortBy);
            return data;
        }

        // GET: api/SensorData/filter
        [HttpGet("filter")] 
        public async Task<List<SensorDataReadDto>> GetFiltered(
            [FromQuery] string? sensorType,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? sortBy)
        {
            var data = await _mongoDBService.GetFilteredDataAsync(sensorType, startDate, endDate, sortBy);
            return data;
        }

        // POST: api/SensorData
        [HttpPost]
        public async Task<IActionResult> Post(SensorData data)
        {
            await _mongoDBService.InsertSensorData(data);
            return CreatedAtAction(nameof(Get), new { id = data.Id }, data);
        }
    }
}