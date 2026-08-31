using SharpPcap;

namespace TelemetryDeviceV1.Dataflow.Abstractions
{
    public interface IPacketHandler
    {
        void HandlePacket(RawCapture packet);
        void Complete();
        void Fault(Exception ex);
    }
}
