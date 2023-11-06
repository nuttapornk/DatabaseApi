using DatabaseApi.Models;
using DatabaseApi.MessageBrokers.Kafka;

namespace DatabaseApi.MessageBrokers;

public static class MessageBrokersCollectionExtension
{
    public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services, KafkaOptions options)
    {
        services.AddKafkaProducer<T>(options);
        return services;
    }

    private static IServiceCollection AddKafkaProducer<T>(this IServiceCollection services, KafkaOptions options)
    {
        services.AddSingleton<IKafkaProducer<T>>(
            new KafkaProducer<T>(
                options.BootstrapServices,
                options.Topics[typeof(T).Name]
            )
        );
        return services;
    }

}
