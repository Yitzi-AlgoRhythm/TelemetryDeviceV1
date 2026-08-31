using System.Threading.Tasks.Dataflow;
using SharpPcap;
using TelemetryDeviceV1.Dataflow.Abstractions;

namespace TelemetryDeviceV1.Dataflow
{
    public class Pipeline : IPacketHandler
    {
        private readonly ITargetBlock<RawCapture> _entryBlock;
        public Task Completion { get; init; }

        public Pipeline(ITargetBlock<RawCapture> entryBlock, Task completion)
        {
            _entryBlock = entryBlock ?? throw new ArgumentNullException(nameof(entryBlock));
            Completion = completion ?? throw new ArgumentNullException(nameof(completion));
        }

        public void HandlePacket(RawCapture packet)
        {
            _entryBlock.Post(packet);
        }

        public void Complete()
        {
            _entryBlock.Complete();
        }

        public void Fault(Exception ex)
        {
            _entryBlock.Fault(ex);
        }
    }
}
