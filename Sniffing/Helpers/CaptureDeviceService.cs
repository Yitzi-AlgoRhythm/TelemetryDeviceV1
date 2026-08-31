using Microsoft.Extensions.Options;
using SharpPcap;
using SharpPcap.LibPcap;
using TelemetryDeviceV1.Config;

namespace TelemetryDeviceV1.Sniffing.Helpers
{
    public class CaptureDeviceService
    {
        private readonly string CaptureDeviceDescription;

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

        public CaptureDeviceService(IOptions<CaptureDeviceConfig> options)
        {
            CaptureDeviceDescription = options.Value.DeviceDesc;
        }

        private LibPcapLiveDevice GetDeviceByDescription(LibPcapLiveDeviceList devices)
        {
            LibPcapLiveDevice? device = null;

            foreach (LibPcapLiveDevice d in devices)
            {
                if (d.Description == CaptureDeviceDescription)
                {
                    device = d;
                }
            }

            if (device == null)
            {
                throw new Exception("Correct capture device not found.");
            }

            return device;
        }
    }
}
