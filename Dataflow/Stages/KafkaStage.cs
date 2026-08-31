using System.Text.Json;
using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class KafkaStage : IKafkaStage
    {
        private readonly TelemetryProducer _producer;

        public KafkaStage(TelemetryProducer producer)
        {
            _producer = producer;
        }

        public async Task Transmit(ParameterData parameterValue)
        {
            string json = JsonSerializer.Serialize(parameterValue);

            await _producer.SendTelemetryAsync(json);
        }
    }
}