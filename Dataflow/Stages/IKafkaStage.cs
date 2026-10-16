using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IKafkaStage
    {
        public void Transmit(List<ParameterData> parameters);

        public void FlushProducer();
    }
}
