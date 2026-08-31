using System.Threading.Tasks.Dataflow;
using ParameterDataLib;
using SharpPcap;
using TelemetryDeviceV1.Dataflow.Stages;

namespace TelemetryDeviceV1.Dataflow
{
    public class PipelineBuilder
    {
        private readonly IBuilderStage _builder;
        private readonly IDecoderStage _decoder;
        private readonly IKafkaStage _kafka;

        public PipelineBuilder(IBuilderStage builder, IDecoderStage decoder, IKafkaStage kafka)
        {
            _builder = builder;
            _decoder = decoder;
            _kafka = kafka;
        }

        public Pipeline Build()
        {
            TransformBlock<RawCapture, IEnumerable<byte>> builderBlock = new TransformBlock<RawCapture, IEnumerable<byte>>
            (
                _builder.Build,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }
            );

            TransformManyBlock<IEnumerable<byte>, ParameterData> decoderBlock = new TransformManyBlock<IEnumerable<byte>, ParameterData>
            (
                _decoder.Decode,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }
            );

            ActionBlock<ParameterData> kafkaBlock = new ActionBlock<ParameterData>
            (
                _kafka.Transmit,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 }
            );

            DataflowLinkOptions options = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            builderBlock.LinkTo(decoderBlock, options, result => result != null);
            decoderBlock.LinkTo(kafkaBlock, options);

            builderBlock.LinkTo(DataflowBlock.NullTarget<IEnumerable<byte>>());

            return new Pipeline(entryBlock: builderBlock, completion: kafkaBlock.Completion);
        }
    }
}
