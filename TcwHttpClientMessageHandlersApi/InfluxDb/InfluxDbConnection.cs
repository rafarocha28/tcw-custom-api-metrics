using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;
using TcwHttpClientMessageHandlersApi.Options;

namespace TcwHttpClientMessageHandlersApi.Influx
{
    public class InfluxDbConnection : IInfluxDbConnection
    {
        private readonly ILogger<InfluxDbConnection> _logger;
        private readonly InfluxDbOptions _options;
        public InfluxDbConnection(ILogger<InfluxDbConnection> logger, IOptionsMonitor<InfluxDbOptions> options)
        {
            _logger = logger;
            _options = options.CurrentValue;
        }

        private InfluxDBClient GetClient()
        {
            return new InfluxDBClient(
                _options.Url, 
                _options.Username,
                _options.Password, 
                _options.Database, 
                "autogen"
            );
        }

        public async Task<bool> WriteData(string tag, long elapsedMs, DateTime timestamp, 
            CancellationToken cancellationToken)
        {
            var influxDBClient = GetClient();
            try
            {
                var point = PointData.Measurement("tcw_endpoint_reply_time_client")
                    .Tag("requestUri", tag)
                    .Field("value", elapsedMs)
                    .Timestamp(timestamp, WritePrecision.Ns);

                await influxDBClient.GetWriteApiAsync().WritePointAsync(point, cancellationToken: cancellationToken);
                _logger.LogInformation("Data sent to InfluxDb");
                return true;
            }
            finally
            {
                influxDBClient.Dispose();
            }
        }
    }
}
