using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public interface IDecoderStage
    {
        public IEnumerable<List<ParameterData>> Decode(byte[] data);
    }
}
