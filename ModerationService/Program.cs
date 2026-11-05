using ModerationService.Configurations;

namespace ModerationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

			// Add services to the container.

			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			ConfigureServicesDatabase.ConfigureServices(builder.Services, config);
			ConfigureServicesLogger.ConfigureServices(builder);
			ConfigureServicesLogic.ConfigureServices(builder.Services, config);
            ConfigureServicesRabbitMQ.ConfigureServices(builder.Services, config);
            ConfigureServicesFilter.ConfigureServices();

            var app = builder.Build();

			ConfigureServicesDatabase.Configure(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
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
