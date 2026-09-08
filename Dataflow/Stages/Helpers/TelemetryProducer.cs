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
                Acks = Acks.All,
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

            try
            {
                DeliveryResult<string, string> result = await producer.ProduceAsync("telemetry-topic", message);
                Console.WriteLine($"Delivered to {result.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
                throw;
            }
        }

        public void Dispose()
        {
            producer.Flush(TimeSpan.FromSeconds(10));
            producer.Dispose();
        }
    }
}
