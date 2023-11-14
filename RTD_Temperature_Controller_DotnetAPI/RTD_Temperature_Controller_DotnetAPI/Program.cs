
using Contracts;
using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //database
            builder.Services.AddDbContext<RTDSensorDBContext>(
                options => {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

            //dependency injection (IOC Service) for Serial port
            builder.Services.AddSingleton<ISerialPortService, SerialPortService>();

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

            app.MapControllers();

            //app.MapGet("/config", () => app.Configuration["ConnectionStrings:ConStr"] + " " + app.Configuration["Credentials:password"]);

            app.Run();
        }
    }
}