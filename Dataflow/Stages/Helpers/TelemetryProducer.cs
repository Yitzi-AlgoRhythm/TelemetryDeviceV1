using Confluent.Kafka;
using Microsoft.Extensions.Options;
using TelemetryDeviceV1.Config;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages.Helpers
{
    public class TelemetryProducer : IDisposable
    {
        private readonly string bootstrapServers;
        private readonly string topicName;
        private readonly long pollWaitTime;

        private readonly IProducer<string, string> _producer;

        private readonly ILoggerTD _logger;

        public TelemetryProducer(IOptions<KafkaConfig> options, ILoggerTD logger)
        {
            bootstrapServers = options.Value.BootstrapServers;
            topicName = options.Value.TopicName;
            pollWaitTime = 0;

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true,
                LingerMs = 10,
                CompressionType = CompressionType.Lz4
            };

            _producer = new ProducerBuilder<string, string>(config).Build();

            _logger = logger;
        }

        public void SendTelemetryAsync(string jsonPayload)
        {
            Message<string, string> message = new Message<string, string>
            {
                Value = jsonPayload
            };

            _producer.Produce(topicName, message, DeliveryHandler);
            _producer.Poll(new TimeSpan(pollWaitTime));
        }

        private void DeliveryHandler(DeliveryReport<string, string> report)
        {
            if (report.Error.IsError)
            {
                _logger.Log(report.Error.Reason);
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
