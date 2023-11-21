
using Contracts;
using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using Services;
using System.IO.Ports;

namespace RTD_Temperature_Controller_DotnetAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //websocket
            builder.Services.AddSignalR();

            //database
            builder.Services.AddDbContext<RTDSensorDBContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
<<<<<<< HEAD
                },ServiceLifetime.Singleton);
=======
                }, ServiceLifetime.Singleton);
>>>>>>> 6edb0b5c51109136db54897da82c1927f0c6434f

            //dependency injection (IOC Service) for Serial port
            builder.Services.AddSingleton<ISerialPortService, SerialPortService>();
            builder.Services.AddScoped<IDataService, DataService>();

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

            app.MapHub<TemperatureHub>("/temperatureHub");

            app.MapControllers();



            //app.MapGet("/config", () => app.Configuration["ConnectionStrings:ConStr"] + " " + app.Configuration["Credentials:password"]);

            app.Run();
        }
    }
}