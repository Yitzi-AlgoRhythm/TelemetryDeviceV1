using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IDecoderStage
    {
        public IEnumerable<ParameterData> Decode(IEnumerable<byte> data);
    }
}
