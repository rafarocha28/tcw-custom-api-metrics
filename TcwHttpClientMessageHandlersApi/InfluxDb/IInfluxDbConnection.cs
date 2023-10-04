namespace TcwHttpClientMessageHandlersApi.Influx
{
    public interface IInfluxDbConnection
    {
        Task<bool> WriteData(string tag, long elapsedMs, DateTime timestamp, CancellationToken cancellationToken);
    }
}
