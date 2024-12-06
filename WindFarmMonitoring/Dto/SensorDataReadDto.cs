namespace WindFarmMonitoring.Dto
{
    public class SensorDataReadDto
    {
        public string SensorType { get; set; }
        public int SensorId { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }

        public SensorDataReadDto()
        {
            SensorType = string.Empty;
        }
    }
}