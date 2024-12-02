using MQTTnet;
using MQTTnet.Client;
using WindFarmMonitoring.Data;
using WindFarmMonitoring.Models;
using System.Text.Json;

namespace WindFarmMonitoring.Services
{
    public class MqttService
    {
        private readonly MongoDBService _mongoDBService;
        private IMqttClient _mqttClient;

        public MqttService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task StartAsync()
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("WindFarmMonitoringClient")
                .WithTcpServer("localhost", 1883)
                .WithCleanSession()
                .Build();

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Połączono z MQTT Brokerem.");
                await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter("windfarm/sensors/#")
                    .Build());
                Console.WriteLine("Subskrypcja tematu windfarm/sensors/# zakończona sukcesem.");
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine($"Rozłączono z MQTT Brokerem");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await _mqttClient.ConnectAsync(mqttOptions, System.Threading.CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas ponownego łączenia: {ex.Message}");
                }
            };

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Otrzymano wiadomość: {message}");

                try
                {
                    var sensorData = JsonSerializer.Deserialize<SensorData>(message);

                    if (sensorData != null)
                    {
                        await _mongoDBService.InsertSensorData(sensorData);
                        Console.WriteLine("Dane sensora zapisane do MongoDB.");
                    }
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Błąd deserializacji wiadomości: {jsonEx.Message}");
                }
            };

            try
            {
                await _mqttClient.ConnectAsync(mqttOptions, System.Threading.CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas łączenia z MQTT Brokerem: {ex.Message}");
            }
        }

        public async Task StopAsync()
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync();
                Console.WriteLine("Rozłączono z MQTT Brokerem.");
            }
        }
    }
}