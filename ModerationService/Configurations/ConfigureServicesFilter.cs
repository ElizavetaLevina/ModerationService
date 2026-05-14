using BogaNet.BWF;
using BogaNet.BWF.Filter;

namespace ModerationService.Configurations
{
    /// <summary>
    /// Конфигурация сервиса фильтрации нецензурной лексики
    /// </summary>
    public static class ConfigureServicesFilter
    {
        /// <summary>
        /// Загружает словари
        /// </summary>
        public static void ConfigureServices()
        {
            BadWordFilter.Instance.LoadFiles(true, BWFConstants.BWF_LTR);
        }
    }
}
