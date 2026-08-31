using PacketDotNet;
using SharpPcap;
using TelemetryDeviceV1.Deserialization;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class BuilderStage : IBuilderStage
    {
        public IEnumerable<byte> Build(RawCapture capture)
        {
            Packet packet = Packet.ParsePacket(capture.LinkLayerType, capture.Data);

            UdpPacket udp = packet.Extract<UdpPacket>();

            if (udp == null || udp.PayloadData.Length < 3)
            {
                return Enumerable.Empty<byte>();
            }

            for (int i = 0; i < IcdConstants.SyncBytes.Length; i++)
            {
                if (IcdConstants.SyncBytes[i] != udp.PayloadData[i])
                {
                    return Enumerable.Empty<byte>();
                }
            }

            ulong timestampMS = (capture.Timeval.Seconds * 1000)
                + (capture.Timeval.MicroSeconds / 1000);

            return [.. BitConverter.GetBytes(timestampMS), .. udp.PayloadData.Skip(IcdConstants.SyncBytes.Length)];
        }
    }
}
