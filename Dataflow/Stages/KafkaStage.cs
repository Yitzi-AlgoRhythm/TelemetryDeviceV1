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

        public void FlushProducer()
        {
            _producer.Flush();
        }

        public void Transmit(List<ParameterData> parameterValues)
        {
            string json;

            json = JsonSerializer.Serialize(parameterValues);

            Console.WriteLine("Sending to kafka");

            _producer.SendTelemetryAsync(json);
        }
    }
}