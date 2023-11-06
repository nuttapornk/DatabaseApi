namespace DatabaseApi.MessageBrokers.Kafka;

public interface IKafkaProducer<T>
{
    Task SendAsync(string key, T message, CancellationToken cancellationToken = default);
}
