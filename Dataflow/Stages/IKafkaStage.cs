using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IKafkaStage
    {
        public void Transmit(ParameterData parameter);
    }
}
