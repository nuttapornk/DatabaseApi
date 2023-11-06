using Confluent.Kafka;
using Newtonsoft.Json;

namespace DatabaseApi.MessageBrokers.Kafka;

public class KafkaProducer<T> : IKafkaProducer<T>, IDisposable
{
    const int DEFAULT_MESSAGE_SIZE = 1024 * 1024 * 10;
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;
    public KafkaProducer(string bootstrapServices, string topic)
    {
        _topic = topic;
        _producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = bootstrapServices,
            ClientId = System.Net.Dns.GetHostName(),
            MessageMaxBytes = DEFAULT_MESSAGE_SIZE
        }).Build();
    }

    public async Task SendAsync(string key, T message, CancellationToken cancellationToken = default)
    {
        var value = JsonConvert.SerializeObject(message, Formatting.Indented);
        await _producer.ProduceAsync(_topic, new Message<string, string>
        {
            Key = key,
            Value = value
        }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(1));
        _producer.Dispose();
    }
}
