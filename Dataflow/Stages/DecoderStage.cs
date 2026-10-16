using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.ICD;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class DecoderStage : IDecoderStage
    {
        private readonly IcdDeserializer _deserializer;

        public DecoderStage(IcdDeserializer deserializer)
        {
            _deserializer = deserializer;
        }
        
        public IEnumerable<List<ParameterData>> Decode(byte[] data)
        {
            if (data is null)
            {
                return [];
            }

            PacketData packetData = new(data);

            List<ParameterData> result = [];

            foreach (IcdParameter icdParam in _deserializer.CorrelatorGroups[packetData.CorrelatorByte])
            {
                result.Add(packetData.GetParameterData(icdParam));
            }

            return [result];
        }
    }
}
