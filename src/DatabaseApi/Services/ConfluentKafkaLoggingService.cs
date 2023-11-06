using System.Text.RegularExpressions;
using DatabaseApi.MessageBrokers.Kafka;
using DatabaseApi.Models;

namespace DatabaseApi.Services;

public class ConfluentKafkaLoggingService : IConfluentKafkaLoggingService
{
    const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
    const string KAFKA_INDEX_CONFIG_KEY = "KafkaOptions:Index";
    private readonly ILogger _logger;
    private readonly IKafkaProducer<MessageLogger> _kafkaMessageLoggerProducer;
    public ConfluentKafkaLoggingService(IConfiguration configuration,
    ILogger<ConfluentKafkaLoggingService> logger,
    IKafkaProducer<MessageLogger> kafkaMessageLoggerProducer)
    {
        _logger = logger;
        _kafkaMessageLoggerProducer = kafkaMessageLoggerProducer;

        string kafkaIndex = configuration[KAFKA_INDEX_CONFIG_KEY] ?? "";
        string indexFormat = string.Empty;

        GetKafkaIndex(ref kafkaIndex, ref indexFormat);
    }

    public async Task InfoAsync(MessageLogger message, string traceId)
    {
        try
        {

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        await Task.CompletedTask;
    }

    public async Task ErrorAsync(MessageLogger message, string traceId)
    {
        try
        {
            await _kafkaMessageLoggerProducer.SendAsync(traceId, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        await Task.CompletedTask;
    }

    private void GetKafkaIndex(ref string index, ref string format)
    {
        try
        {
            var match = Regex.Match(index, @"{.*?}");
            var trimChars = new[] { '-', '{', '}' };
            var separator = new[] { ':' };

            if (match.Success)
            {
                format = match.Value.Trim(trimChars).Split(separator)[1];
                index = index.Replace(match.Value, string.Empty);
            }
            index = index.TrimStart(trimChars).TrimEnd(trimChars);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
