using MongoDB.Driver;
using WindFarmMonitoring.Models;
using AutoMapper;
using WindFarmMonitoring.Dto;

namespace WindFarmMonitoring.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<SensorData> _sensorDataCollection;
        private readonly IMapper _mapper;

        public MongoDBService(IConfiguration config, IMapper mapper)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("WindFarmDB");
            _sensorDataCollection = database.GetCollection<SensorData>("SensorData");
            _mapper = mapper;
        }

        public async Task InsertSensorData(SensorData data)
        {
            await _sensorDataCollection.InsertOneAsync(data);
        }

        public async Task<List<SensorDataReadDto>> GetSensorDataAsync(string? sortBy)
        {
            var sortDefinition = GetSortDefinition(sortBy);
            var sensorData = await _sensorDataCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
            return _mapper.Map<List<SensorDataReadDto>>(sensorData);
        }

        public async Task<List<SensorDataReadDto>> GetFilteredDataAsync(
            string? sensorType, DateTime? startDate, DateTime? endDate, string? sortBy)
        {
            var filterBuilder = Builders<SensorData>.Filter;
            var filters = new List<FilterDefinition<SensorData>>();

            if (!string.IsNullOrEmpty(sensorType))
                filters.Add(filterBuilder.Eq(s => s.SensorType, sensorType));

            if (startDate.HasValue)
                filters.Add(filterBuilder.Gte(s => s.Timestamp, startDate.Value));

            if (endDate.HasValue)
                filters.Add(filterBuilder.Lte(s => s.Timestamp, endDate.Value));

            var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

            var sortDefinition = GetSortDefinition(sortBy);

            var sensorData = await _sensorDataCollection.Find(combinedFilter).Sort(sortDefinition).ToListAsync();
            return _mapper.Map<List<SensorDataReadDto>>(sensorData);
        }

        private SortDefinition<SensorData>? GetSortDefinition(string? sortBy)
        {
            var sortBuilder = Builders<SensorData>.Sort;

            return sortBy?.ToLower() switch
            {
                "sensorid" => sortBuilder.Ascending(s => s.SensorId),
                "sensortype" => sortBuilder.Ascending(s => s.SensorType),
                "value" => sortBuilder.Ascending(s => s.Value),
                "timestamp" => sortBuilder.Ascending(s => s.Timestamp),
                _ => null,
            };
        }
    }
}