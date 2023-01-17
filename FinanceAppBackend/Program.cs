using Microsoft.EntityFrameworkCore;
using FinanceAppBackend.Contexts;
using FinanceAppBackend.Models;
using FinanceAppBackend.Helpers;

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Configuration.AddConfiguration(configurationBuilder.Build());

// Add services to the container.

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddCors();

var defaultConnectionString = string.Empty;

if (builder.Environment.EnvironmentName == "Development") {
    defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
    // Use connection string provided at runtime by Heroku.
    var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
    var userPassSide = connectionUrl.Split("@")[0];
    var hostSide = connectionUrl.Split("@")[1];

    var user = userPassSide.Split(":")[0];
    var password = userPassSide.Split(":")[1];
    var host = hostSide.Split("/")[0];
    var database = hostSide.Split("/")[1].Split("?")[0];

    defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<UserContext>(options =>
options.UseNpgsql(defaultConnectionString));

var serviceProvider = builder.Services.BuildServiceProvider();
try
{
    var dbContext = serviceProvider.GetRequiredService<UserContext>();
    dbContext.Database.Migrate();
}
catch
{
}

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PsqlQuery>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(options => options
    .WithOrigins(new []{"https://finance-app-fe.herokuapp.com/","http://localhost:3000","http://localhost:8080","http://localhost:4200"})
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.MapControllers();

app.Run();
