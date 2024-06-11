var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddSingleton(builder);
builder.Services.AddControllers();

// Configure app configuration
builder.Configuration.AddJsonFile( "configuration.json");

// Use Startup
//builder.Services.AddStartup<Startup>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
