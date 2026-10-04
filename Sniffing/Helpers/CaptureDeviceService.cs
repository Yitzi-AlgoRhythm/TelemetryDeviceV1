using Microsoft.Extensions.Options;
using SharpPcap;
using SharpPcap.LibPcap;
using TelemetryDeviceV1.Config;

namespace TelemetryDeviceV1.Sniffing.Helpers
{
    public class CaptureDeviceService
    {
        private readonly string _captureDeviceDescription;
        private readonly string _sourceIP;
        private readonly string _destPort;

        public required string DeviceFilter { get; init; }

        public LibPcapLiveDevice Device
        {
            get
            {
                LibPcapLiveDeviceList devices = LibPcapLiveDeviceList.Instance;

                if (devices.Count == 0)
                {
                    throw new Exception("No capture-capable network devices found.");
                }

                return GetDeviceByDescription(devices);
            }
        }

        public CaptureDeviceService(IOptions<CaptureConfig> options)
        {
            _captureDeviceDescription = options.Value.DeviceDesc;
            _sourceIP = options.Value.SourceIP;
            _destPort = options.Value.DestPort;

            DeviceFilter = $"src host {_sourceIP} and dst port {_destPort}";
        }

        private LibPcapLiveDevice GetDeviceByDescription(LibPcapLiveDeviceList devices)
        {
            LibPcapLiveDevice? device = null;

            foreach (LibPcapLiveDevice d in devices)
            {
                if (d.Description == _captureDeviceDescription)
                {
                    device = d;

                    return device;
                }
            }

            throw new Exception("Correct capture device not found.");
        }
    }
}
