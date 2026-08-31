using SharpPcap;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IBuilderStage
    {
        public IEnumerable<byte> Build(RawCapture capture);
    }
}
