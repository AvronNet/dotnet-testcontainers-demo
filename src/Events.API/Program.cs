using Events.Api;
using Events.Core.Events;
using Events.Infrastructure.DB;
using Events.Infrastructure.DB.Context;
using Events.Infrastructure.DB.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

StartupUtils.StartTestcontainerForLocalDev();

builder.Configuration.AddEnvironmentVariables();

// Get configurations
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DB context
builder.Services.AddDbContext<EventsDBContext>(options => options.UseSqlServer(connectionString, b =>
{
    b.MigrationsAssembly("Events.Infrastructure");
    b.EnableRetryOnFailure();
}));

// Add Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisUrl");
});

builder.Services.AddAutoMapper(typeof(EntityMapperProfile).Assembly);

// Add services
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<EventService>();


// Add an any origin CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() 
    || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local"
    || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Apply the CORS policy
app.UseCors("AllowAnyOrigin");

app.MapControllers();

// Apply migrations
StartupUtils.ApplyMigrations(app);

app.Run();

public partial class Program { }
