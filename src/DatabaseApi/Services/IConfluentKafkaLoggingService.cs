using DatabaseApi.Models;

namespace DatabaseApi.Services;

public interface IConfluentKafkaLoggingService
{
    Task InfoAsync(MessageLogger message, string traceId);

    Task ErrorAsync(MessageLogger message, string traceId);

}
