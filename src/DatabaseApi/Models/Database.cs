namespace DatabaseApi.Models;

public class Database
{
    public string Name { get; set; } = string.Empty;
    public int? Port { get; set; }
    public string Server { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
