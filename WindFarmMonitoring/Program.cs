using WindFarmMonitoring.Services;
using WindFarmMonitoring.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<MqttService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();

var mqttService = app.Services.GetRequiredService<MqttService>();
await mqttService.StartAsync();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();