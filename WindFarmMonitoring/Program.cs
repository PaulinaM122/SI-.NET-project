using WindFarmMonitoring.Services;
using WindFarmMonitoring.Data;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja usług
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<MqttService>();

// Rejestracja kontrolerów i autoryzacji
builder.Services.AddControllers();
builder.Services.AddAuthorization(); // Dodanie usługi autoryzacji

var app = builder.Build();

// Uruchomienie MqttService
var mqttService = app.Services.GetRequiredService<MqttService>();
await mqttService.StartAsync();

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization(); // Middleware autoryzacji
app.MapControllers();
app.Run();