using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WindFarmMonitoring.Models
{
    public class SensorData
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("sensorType")]
        public string SensorType { get; set; }

        [BsonElement("sensorId")]
        public int SensorId { get; set; }

        [BsonElement("value")]
        public double Value { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        public SensorData() 
        { 
            SensorType = string.Empty;
        }
    }
}
