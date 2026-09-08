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
            Console.WriteLine($"Serializing parameter {parameterValue.Name}");

            string json;

            try
            {
                json = JsonSerializer.Serialize(parameterValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            

            Console.WriteLine("Sending JSON:");
            Console.WriteLine(json);

            await _producer.SendTelemetryAsync(json);
        }
    }
}