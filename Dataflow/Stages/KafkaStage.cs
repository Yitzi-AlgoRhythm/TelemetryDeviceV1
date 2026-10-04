using System.Text.Json;
using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class KafkaStage : IKafkaStage
    {
        private readonly TelemetryProducer _producer;

        public KafkaStage(TelemetryProducer producer, ILoggerTD logger)
        {
            _producer = producer;
        }

        public void FlushProducer()
        {
            _producer.Flush();
        }

        public void Transmit(ParameterData parameterValue)
        {
            string json;

            json = JsonSerializer.Serialize(parameterValue);

            _producer.SendTelemetryAsync(json);
        }
    }
}