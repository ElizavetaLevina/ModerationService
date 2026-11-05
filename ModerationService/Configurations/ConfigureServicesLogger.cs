using Serilog;

namespace ModerationService.Configurations
{
    /// <summary>
    /// Конфигурация логирования с использованием Serilog
    /// </summary>
    public static class ConfigureServicesLogger
    {
        /// <summary>
        /// Настраивает Serilog
        /// </summary>
        /// <param name="builder">WebApplicationBuilder для настройки Host с Serilog</param>
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
