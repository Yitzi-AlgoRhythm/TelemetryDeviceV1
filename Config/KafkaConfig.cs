namespace TelemetryDeviceV1.Config
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string TopicName { get; set; } = string.Empty;
    }
}
