namespace TelemetryDeviceV1.Sniffing
{
    public interface INetworkSniffer
    {
        public void BeginCapture(CancellationToken token);
    }
}
