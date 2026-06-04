using ModerationService.BLL;
using ModerationService.Configurations;

namespace ModerationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            var isInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			ConfigureServicesDatabase.ConfigureServices(builder.Services, config, isInDocker);
			ConfigureServicesLogger.ConfigureServices(builder, isInDocker);
			ConfigureServicesAutoMapper.ConfigureServices(builder.Services);
			ConfigureServicesLogic.ConfigureServices(builder.Services);
            ConfigureServicesRabbitMQ.ConfigureServices(builder.Services, config, isInDocker);
            ConfigureServicesFilter.ConfigureServices();

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
        }
    }
}
