using System.Diagnostics;
using TcwHttpClientMessageHandlersApi.Influx;

namespace TcwHttpClientMessageHandlersApi.Handlers
{
    public sealed class TimeHandler: DelegatingHandler
    {
        private readonly ILogger<TimeHandler> _logger;
        private readonly IInfluxDbConnection _influxDbConnection;
        public TimeHandler(ILogger<TimeHandler> logger, IInfluxDbConnection influxDbConnection) 
        { 
            _logger = logger;
            _influxDbConnection = influxDbConnection;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Time-Handler", "time-handler");
            var requestUri = request.RequestUri?.PathAndQuery ?? "/v1/alert-type";
            var timestamp = DateTime.Now;
            
            var stopwatch = new Stopwatch();
            _logger.LogInformation("Start: {start}", timestamp.ToString("dd/mm/yyyy HH:MM:ss.ffff"));
            stopwatch.Start();
            var response = await base.SendAsync(request, cancellationToken);            
            stopwatch.Stop();
            _logger.LogInformation("End: {end}", DateTime.Now.ToString("dd/mm/yyyy HH:MM:ss.ffff"));
            _logger.LogInformation("Duration: {duration}ms", stopwatch.Elapsed.TotalMilliseconds);

            await _influxDbConnection.WriteData(requestUri, (long)stopwatch.Elapsed.TotalMilliseconds, timestamp, 
                cancellationToken);
            return response;
        }
    }
}
