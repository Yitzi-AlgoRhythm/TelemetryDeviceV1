using SharpPcap;
using SharpPcap.LibPcap;
using TelemetryDeviceV1.Dataflow.Abstractions;
using TelemetryDeviceV1.Sniffing.Helpers;

namespace TelemetryDeviceV1.Sniffing
{
    public class NetworkSniffer : INetworkSniffer
    {
        private readonly IPacketHandler _packetHandler;
        private readonly CaptureDeviceService _deviceService;

        public NetworkSniffer(IPacketHandler packetHandler, CaptureDeviceService deviceService)
        {
            _packetHandler = packetHandler;
            _deviceService = deviceService;
        }

        public void BeginCapture(CancellationToken token)
        {
            LibPcapLiveDevice device = _deviceService.Device;
            device.OnPacketArrival += OnArrival;

            token.Register(() =>
            {
                device.StopCapture();
                device.OnPacketArrival -= OnArrival;
                _packetHandler.Complete();
            });

            device.Open(DeviceModes.Promiscuous);

            device.StartCapture();
        }

        private void OnArrival(object sender, PacketCapture e)
        {
            try
            {
                _packetHandler.HandlePacket(e.GetPacket());
            }
            catch (Exception ex)
            {
                _packetHandler.Fault(ex);
            }
        }
    }
}
