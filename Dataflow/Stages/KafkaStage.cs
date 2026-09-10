using System.Text.Json;
using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class KafkaStage : IKafkaStage
    {
        private readonly TelemetryProducer _producer;
        private readonly ILoggerTD _logger;

        public KafkaStage(TelemetryProducer producer, ILoggerTD logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task Transmit(ParameterData parameterValue)
        {
            _logger.Log("Kafka");

            string json;

            json = JsonSerializer.Serialize(parameterValue);

            await _producer.SendTelemetryAsync(json);
        }
    }
}