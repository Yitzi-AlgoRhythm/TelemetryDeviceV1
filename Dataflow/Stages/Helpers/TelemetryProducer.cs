using Confluent.Kafka;

namespace TelemetryDeviceV1.Dataflow.Stages.Helpers
{
    public class TelemetryProducer : IDisposable
    {
        private const string bootstrapServers = "localhost:9092";

        private readonly IProducer<string, string> producer;

        public TelemetryProducer()
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.Leader,
                EnableIdempotence = true,
                LingerMs = 5,
                CompressionType = CompressionType.Snappy
            };

            producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task SendTelemetryAsync(string jsonPayload)
        {
            Message<string, string> message = new Message<string, string>
            {
                Value = jsonPayload
            };

            await producer.ProduceAsync("telemetry-topic", message);
        }

        public void Dispose()
        {
            producer.Flush(TimeSpan.FromSeconds(10));
            producer.Dispose();
        }
    }
}
