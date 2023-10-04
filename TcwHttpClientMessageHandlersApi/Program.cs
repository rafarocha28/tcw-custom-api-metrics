using System.Reflection;
using TcwHttpClientMessageHandlersApi.Handlers;
using TcwHttpClientMessageHandlersApi.Influx;
using TcwHttpClientMessageHandlersApi.Options;

var configPath = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                  ?? Path.GetDirectoryName(Environment.ProcessPath))
                 ?? Environment.CurrentDirectory;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(configPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<TimeHandler>();
builder.Services.AddTransient<IInfluxDbConnection, InfluxDbConnection>();
builder.Services.AddHttpClient("time-handler")
    .AddHttpMessageHandler<TimeHandler>();

builder.Services.Configure<TcwOptions>(config.GetSection(TcwOptions.Tcw));
builder.Services.Configure<InfluxDbOptions>(config.GetSection(InfluxDbOptions.InfluxDb));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
