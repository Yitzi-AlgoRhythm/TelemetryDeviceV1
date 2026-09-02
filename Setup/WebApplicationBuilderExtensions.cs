using TelemetryDeviceV1.Config;
using TelemetryDeviceV1.Dataflow;
using TelemetryDeviceV1.Dataflow.Stages;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.ICD;
using TelemetryDeviceV1.Services;
using TelemetryDeviceV1.Sniffing.Helpers;

namespace TelemetryDeviceV1.Setup
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.Configure<CaptureDeviceConfig>
                            (builder.Configuration.GetSection(nameof(CaptureDeviceConfig)));

            builder.Services.AddSingleton<IcdDeserializer>();

            builder.Services.AddSingleton<CaptureDeviceService>();
            builder.Services.AddSingleton<TelemetryProducer>();

            builder.Services.AddSingleton<IBuilderStage, BuilderStage>();
            builder.Services.AddSingleton<IDecoderStage, DecoderStage>();
            builder.Services.AddSingleton<IKafkaStage, KafkaStage>();

            builder.Services.AddSingleton<PipelineBuilder>();

            builder.Services.AddSingleton<TDRuntimeService>();

            return builder;
        }
    }
}
