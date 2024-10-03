using Microsoft.EntityFrameworkCore;
using IoTDataCollector.API;
using IoTDataCollector.API.Services;
using IoTDataCollector.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddDbContext<IoTDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IoTDataService>();
builder.Services.AddHttpClient(); // IHttpClientFactory ekleniyor
builder.Services.AddScoped<HttpClientHelper>(); // HttpClientHelper ekleniyor

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
