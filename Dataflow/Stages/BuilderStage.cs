using PacketDotNet;
using SharpPcap;
using TelemetryDeviceV1.Deserialization;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class BuilderStage : IBuilderStage
    {
        private static readonly int syncBytesCount = IcdConstants.SyncBytes.Length;

        private readonly ILoggerTD _logger;

        public BuilderStage(ILoggerTD logger)
        {
            _logger = logger;
        }

        public IEnumerable<byte> Build(RawCapture capture)
        {
            _logger.Log("Builder");

            Packet packet = Packet.ParsePacket(capture.LinkLayerType, capture.Data);

            UdpPacket udp = packet.Extract<UdpPacket>();

            if (udp == null || udp.PayloadData == null || udp.PayloadData.Length < syncBytesCount)
            {
                return null!;
            }

            for (int i = 0; i < syncBytesCount; i++)
            {
                if (IcdConstants.SyncBytes[i] != udp.PayloadData[i])
                {
                    return null!;
                }
            }

            ulong timestampMS = GetTimestamp(capture.Timeval);

            return [.. BitConverter.GetBytes(timestampMS), .. udp.PayloadData.Skip(IcdConstants.SyncBytes.Length)];
        }

        private static ulong GetTimestamp(PosixTimeval timeval)
        {
            const int millisInSecond = 1000;
            const int microsInMillis = 1000;

            return (timeval.Seconds * millisInSecond) + (timeval.MicroSeconds /  microsInMillis);
        }
    }
}
