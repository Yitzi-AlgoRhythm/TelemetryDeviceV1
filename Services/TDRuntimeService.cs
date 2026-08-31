using TelemetryDeviceV1.Dataflow;
using TelemetryDeviceV1.Sniffing;
using TelemetryDeviceV1.Sniffing.Helpers;

namespace TelemetryDeviceV1.Services
{
    public class TDRuntimeService
    {
        private readonly PipelineBuilder _pipelineBuilder;
        private readonly CaptureDeviceService _deviceService;
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(1, 1);

        private CancellationTokenSource? _cts;
        private Pipeline? _pipeline;
        private NetworkSniffer? _sniffer;

        public bool IsRunning => _cts is not null;

        public TDRuntimeService(PipelineBuilder pipelineBuilder, CaptureDeviceService deviceService)
        {
            _pipelineBuilder = pipelineBuilder;
            _deviceService = deviceService;
        }

        public async Task StartAsync()
        {
            await _sem.WaitAsync();

            try
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("Already running");
                }

                _cts = new CancellationTokenSource();
                _pipeline = _pipelineBuilder.Build();
                _sniffer = new NetworkSniffer(_pipeline, _deviceService);

                _sniffer.BeginCapture(_cts.Token);
            }
            finally
            {
                _sem.Release();
            }
        }

        public async Task StopAsync()
        {
            await _sem.WaitAsync();

            try
            {
                if (!IsRunning)
                {
                    throw new InvalidOperationException("Already stopped");
                }

                _cts!.Cancel();

                await _pipeline!.Completion;

                _cts.Dispose();
                _cts = null;
                _pipeline = null;
                _sniffer = null;
            }
            finally
            {
                _sem.Release();
            }
        }
    }
}
