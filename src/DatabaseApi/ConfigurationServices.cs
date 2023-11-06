using DatabaseApi.MessageBrokers;
using DatabaseApi.Models;
using DatabaseApi.Services;

namespace DatabaseApi;

public static class ConfigurationServices
{
    public static IServiceCollection ConfigKafkaLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConfluentKafkaLoggingService, ConfluentKafkaLoggingService>();

        KafkaOptions options = new();
        configuration.GetSection("KafkaOptions").Bind(options);

#if DEBUG
        options.BootstrapServices = options.BootstrapServicesDebug;
#endif

        services.AddMessageBusSender<MessageLogger>(options);

        return services;
    }
}