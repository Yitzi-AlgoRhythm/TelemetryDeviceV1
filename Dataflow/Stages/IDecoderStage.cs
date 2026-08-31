using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IDecoderStage
    {
        public ParameterData Decode(IEnumerable<byte> data);
    }
}
