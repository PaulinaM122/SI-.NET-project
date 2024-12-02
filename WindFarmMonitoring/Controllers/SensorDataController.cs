using Microsoft.AspNetCore.Mvc;
using WindFarmMonitoring.Data;
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
        public async Task<IActionResult> Get()
        {
            var data = await _mongoDBService.GetSensorDataAsync();
            return Ok(data);
        }

        // GET: api/SensorData/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] string sensorType, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var data = await _mongoDBService.GetFilteredDataAsync(sensorType, startDate, endDate);
            return Ok(data);
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