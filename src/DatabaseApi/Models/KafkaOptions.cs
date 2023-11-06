namespace DatabaseApi.Models;

public class KafkaOptions
{
    public string BootstrapServices { get; set; } = string.Empty;
    public string BootstrapServicesDebug { get; set; } = string.Empty;
    public string Index { get; set; } = string.Empty;
    public Dictionary<string, string> Topics { get; set; } = new Dictionary<string, string>();
}
