namespace BaCS.Application.Integrations.Email.Options;

public class EmailOptions
{
    public string SmtpServer { get; init; }
    public int Port { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string ServiceName { get; init; }
}
