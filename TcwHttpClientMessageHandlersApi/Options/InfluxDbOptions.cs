namespace TcwHttpClientMessageHandlersApi.Options
{
    public sealed class InfluxDbOptions
    {
        public const string InfluxDb = "InfluxDb";

        public string Url { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Database { get; set; } = "telegraf";
    }
}