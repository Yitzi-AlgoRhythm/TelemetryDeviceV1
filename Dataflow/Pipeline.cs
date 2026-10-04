using System.Threading.Tasks.Dataflow;
using SharpPcap;
using TelemetryDeviceV1.Dataflow.Abstractions;
using TelemetryDeviceV1.Dataflow.Stages;

namespace TelemetryDeviceV1.Dataflow
{
    public class Pipeline : IPacketHandler
    {
        private readonly ITargetBlock<RawCapture> _entryBlock;
        private readonly IKafkaStage _kafkaStage;
        public Task Completion { get; init; }

        public Pipeline(ITargetBlock<RawCapture> entryBlock, Task completion, IKafkaStage kafkaStage)
        {
            _entryBlock = entryBlock ?? throw new ArgumentNullException(nameof(entryBlock));
            Completion = completion ?? throw new ArgumentNullException(nameof(completion));
            _kafkaStage = kafkaStage ?? throw new ArgumentNullException(nameof(kafkaStage));
        }

        public void HandlePacket(RawCapture packet)
        {
            _entryBlock.Post(packet);
        }

        public void Complete()
        {
            _entryBlock.Complete();
            _kafkaStage.FlushProducer();
        }

        public void Fault(Exception ex)
        {
            _entryBlock.Fault(ex);
        }
    }
}
