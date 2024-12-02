using MongoDB.Driver;
using WindFarmMonitoring.Models;
using Microsoft.Extensions.Configuration;

namespace WindFarmMonitoring.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<SensorData> _sensorDataCollection;

        public MongoDBService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("WindFarmDB");
            _sensorDataCollection = database.GetCollection<SensorData>("SensorData");
        }

        public async Task InsertSensorData(SensorData data)
        {
            await _sensorDataCollection.InsertOneAsync(data);
        }

        public async Task<List<SensorData>> GetSensorDataAsync()
        {
            return await _sensorDataCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<SensorData>> GetFilteredDataAsync(string sensorType, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<SensorData>.Filter.And(
                Builders<SensorData>.Filter.Eq(s => s.SensorType, sensorType),
                Builders<SensorData>.Filter.Gte(s => s.Timestamp, startDate),
                Builders<SensorData>.Filter.Lte(s => s.Timestamp, endDate)
            );
            return await _sensorDataCollection.Find(filter).ToListAsync();
        }
    }
}