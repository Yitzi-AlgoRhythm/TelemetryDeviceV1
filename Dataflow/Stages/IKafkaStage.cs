using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IKafkaStage
    {
        public Task Transmit(ParameterData parameter);
    }
}
