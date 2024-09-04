using System.Collections.Concurrent;

namespace CadastroUsuarioWebApi.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfig loggerConfig;

    readonly ConcurrentDictionary<string, CustomLogger> loggers = 
        new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfig config)
    {
        loggerConfig = config;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
    }

    public void Dispose()
    {
        loggers.Clear();
    }
}
