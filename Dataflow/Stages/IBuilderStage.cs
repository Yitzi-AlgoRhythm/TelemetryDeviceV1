using SharpPcap;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IBuilderStage
    {
        public byte[] Build(RawCapture capture);
    }
}
