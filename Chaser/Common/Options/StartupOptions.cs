namespace Chaser;

public class StartupOptions
{
    public const string Startup = nameof(Startup);

    public string Token { get; init; } = string.Empty;
    public ulong DevelopmentGuildId { get; init; }
}
