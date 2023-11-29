
using Contracts;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using Serilog;
using Services;

namespace RTD_Temperature_Controller_DotnetAPI
{
    /// <summary>
    /// Program class serves as the entry point for the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //websocket
            builder.Services.AddSignalR();

            //dependency injection (IOC Service) for Serial port
            builder.Services.AddSingleton<ISerialPortService, SerialPortService>();
            builder.Services.AddSingleton<IDataService, DataService>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors(o =>
            {
                o.AllowAnyOrigin();
                o.AllowAnyHeader();
                o.AllowAnyMethod();
            });
            app.UseAuthorization();

            // Mapping the SignalR hub for WebSocket communication
            app.MapHub<TemperatureHub>("/temperatureHub");

            app.MapControllers();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
           .WriteTo.Console()
           .WriteTo.File("logs/Logs.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

            app.Run();
        }
    }
}