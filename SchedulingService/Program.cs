using Microsoft.EntityFrameworkCore;
using SchedulingService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Add this line to use Serilog as the logging provider


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SchedulingContext>(options =>
    options.UseNpgsql(connectionString));

// Ensure database is created and migrated
// using (var scope = builder.Services.BuildServiceProvider().CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<SchedulingContext>();
//     // Ensure database is created and apply migrations if there are any pending
//     if (db.Database.GetPendingMigrations().Any())
//     {
//         try
//         {
//             db.Database.Migrate();
//         }
//         catch (Exception ex)
//         {
//             Log.Error(ex, "An error occurred while applying migrations");
//             throw;
//         }
//     }
// }

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();


// Log application start
try
{
    Log.Information("Starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

