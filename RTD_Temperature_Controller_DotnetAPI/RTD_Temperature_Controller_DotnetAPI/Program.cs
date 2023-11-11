
using System.IO.Ports;

namespace RTD_Temperature_Controller_DotnetAPI
{
    public class Program
    {
        public static SerialPort SerialPort;
        public static Thread ReadThread = new Thread(ReadDataFromHardware);

        public static  void ReadDataFromHardware()
        {
            int i = 0;
            while (true)
            {
                try
                {
                    SerialPort.Write("Start ho gaya");
                    string message = SerialPort.ReadLine();
                    Console.WriteLine("FROM HARDWARE: "+message);
                    Console.WriteLine("HI");
                    Thread.Sleep(1000);

                }
                catch (TimeoutException) { }
            }
        }

        public static void Main(string[] args)
        {
            SerialPort = new SerialPort();
            if (SerialPort.IsOpen) { SerialPort.Close(); }


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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

            app.UseCors(o =>
            {
                o.AllowAnyOrigin();
                o.AllowAnyHeader();
                o.AllowAnyMethod();
            });
            app.MapControllers();

            app.Run();
        }
    }
}