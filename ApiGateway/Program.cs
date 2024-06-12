using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("ocelot.json", optional:false, reloadOnChange:true);

builder.Services.AddOcelot(builder.Configuration);
// Use Startup
//builder.Services.AddStartup<Startup>();

var app = builder.Build();
await app.UseOcelot();

// app.MapGet("/", () => "Hello World!");
app.Run();
