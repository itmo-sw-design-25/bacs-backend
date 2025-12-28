namespace BaCS.Presentation.BusinessProcess.Options;

public class ElsaPostgresOptions
{
    public string Username { get; init; }
    public string Password { get; init; }
    public int Port { get; init; }
    public string Host { get; init; }
    public string Database { get; init; }
    public string Schema { get; init; }
    public string AdditionalProperties { get; init; }

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};Include Error Detail=true;{AdditionalProperties}";
}
