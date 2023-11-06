using Dapr.Client;
using DatabaseApi.Models;
using DbUp;
using DbUp.Engine;
using DatabaseApi;
using Microsoft.Data.SqlClient;

const string DAPR_SECRET_STORE = "localsecretstore";
const string SECRET_NAME = "secret";
const string LOCAL_HOST = "127.0.0.1";

var grpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "50001";
var builder = WebApplication.CreateBuilder(args);
var client = new DaprClientBuilder()
.UseGrpcEndpoint($"http://{LOCAL_HOST}:{grpcPort}")
.Build();


builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.ConfigKafkaLogging(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.MapControllers();
app.MapHealthChecks("/health");

List<Database> databases = new();
builder.Configuration.GetSection("Databases").Bind(databases);

if (databases != null && databases.Count > 0)
{
    //var serxxx = builder.Services.BuildServiceProvider().GetServices();
    foreach (var database in databases)
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", database.Name);
        if (Path.Exists(path))
        {
            try
            {
                var secrets = await client.GetSecretAsync(DAPR_SECRET_STORE, $"{SECRET_NAME}");
                string connectionString = GetConnectionString(database, secrets[database.Name]);

                var result = MigrateDatabase(connectionString, path);
            }
            catch (Exception ex)
            {

            }
        }
    }
}

app.Run();

static DatabaseUpgradeResult MigrateDatabase(string connectionString, string path)
{
    EnsureDatabase.For.SqlDatabase(connectionString);
    var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsFromFileSystem(path)
    .LogToConsole()
    .Build();

    return upgrader.PerformUpgrade();
}

static string GetConnectionString(Database database, string secret)
{
    SqlConnectionStringBuilder sqlBuilder = new()
    {
        DataSource = $"{database.Server}:{database.Port ?? 1433}",
        InitialCatalog = database.Name,
        UserID = database.Username,
        Password = secret ?? database.Password,
        TrustServerCertificate = true
    };
    return sqlBuilder.ConnectionString;
}

