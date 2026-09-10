using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.ICD;
using TelemetryDeviceV1.ICD.Enums;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class DecoderStage : IDecoderStage
    {
        private readonly IcdDeserializer _deserializer;

        private readonly ILoggerTD _logger;

        public DecoderStage(IcdDeserializer deserializer, ILoggerTD logger)
        {
            _deserializer = deserializer;
            _logger = logger;
        }
        
        public IEnumerable<ParameterData> Decode(IEnumerable<byte> data)
        {
            _logger.Log("Decoder");

            if (data == Enumerable.Empty<byte>())
            {
                yield break;
            }

            PacketData packetData = new PacketData(data);

            foreach (IcdParameter icdParam in _deserializer.CorrelatorGroups[packetData.CorrelatorByte])
            {
                yield return packetData.GetParameterData(icdParam);
            }
        }
    }
}
