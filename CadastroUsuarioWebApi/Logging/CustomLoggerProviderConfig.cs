namespace CadastroUsuarioWebApi.Logging;

public class CustomLoggerProviderConfig
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public int EventId { get; set; } = 0;
}
