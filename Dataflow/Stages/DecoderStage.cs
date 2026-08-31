using ParameterDataLib;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class DecoderStage : IDecoderStage
    {
        public IEnumerable<ParameterData> Decode(IEnumerable<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}
