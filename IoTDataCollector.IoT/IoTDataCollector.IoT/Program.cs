using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

bool isProducing = false;
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
string deviceId = "device-001";
ConcurrentQueue<object> dataQueue = new ConcurrentQueue<object>();

app.MapPost("/start", () =>
{
    if (!isProducing)
    {
        isProducing = true;
        cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => ProduceData(cancellationTokenSource.Token));
        return Results.Ok("Production started.");
    }
    return Results.BadRequest("Production is already running.");
});

app.MapPost("/stop", () =>
{
    if (isProducing)
    {
        isProducing = false;
        cancellationTokenSource.Cancel();
        return Results.Ok("Production stopped.");
    }
    return Results.BadRequest("Production is not running.");
});

app.MapGet("/data", () =>
{
    if (dataQueue.TryDequeue(out var data))
    {
        return Results.Json(data);
    }
    return Results.NoContent();
});

app.Run();

async Task ProduceData(CancellationToken token)
{
    Random random = new Random();
    using var httpClient = new HttpClient();
    while (!token.IsCancellationRequested)
    {
        var data = new
        {
            DeviceId = deviceId,
            Timestamp = DateTime.UtcNow,
            Value = random.NextDouble() < 0.8 ? 1 : 0
        };

        dataQueue.Enqueue(data);

        var jsonData = JsonSerializer.Serialize(data);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("https://localhost:7083/api/IoTData", content);
            response.EnsureSuccessStatusCode();
            Console.WriteLine($"Produced data: {jsonData}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error posting data: {ex.Message}");
        }

        await Task.Delay(1000, token);
    }
}