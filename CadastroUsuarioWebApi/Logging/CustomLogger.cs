
namespace CadastroUsuarioWebApi.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfig loggerConfig;

        public CustomLogger(string name, CustomLoggerProviderConfig config)
        {
            loggerName = name;
            loggerConfig = config;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
           return logLevel == loggerConfig.LogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            
            EscreverTextoNoArquivo(mensagem);
        }

        public void EscreverTextoNoArquivo(string mensagem)
        {
            string caminhoArquivoLog = @"C:\Users\varle\OneDrive\Documentos\logApi\LogCadastroUsuario.txt"; //exemplo

            using (StreamWriter streamWriter = new StreamWriter (caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine (mensagem);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
