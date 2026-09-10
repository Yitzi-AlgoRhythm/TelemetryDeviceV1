using Confluent.Kafka;
using Microsoft.Extensions.Options;
using TelemetryDeviceV1.Config;

namespace TelemetryDeviceV1.Dataflow.Stages.Helpers
{
    public class TelemetryProducer : IDisposable
    {
        private readonly string bootstrapServers;
        private readonly string topicName;

        private readonly IProducer<string, string> _producer;

        public TelemetryProducer(IOptions<KafkaConfig> options)
        {
            bootstrapServers = options.Value.BootstrapServers;
            topicName = options.Value.TopicName;

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true,
                LingerMs = 5,
                CompressionType = CompressionType.Snappy
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task SendTelemetryAsync(string jsonPayload)
        {
            Message<string, string> message = new Message<string, string>
            {
                Value = jsonPayload
            };

            await _producer.ProduceAsync(topicName, message);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
