using System.Text.Json.Serialization;
using ParameterDataLib;
using TelemetryDeviceV1.ICD.Helpers.TelemetryDeviceV1.Deserialization;
using TelemetryDeviceV1.ICD.Enums;

namespace TelemetryDeviceV1.ICD
{
    [JsonConverter(typeof(IcdParameterConverter))]
    public class IcdParameter
    {
        public required string Name { get; init; }

        public required string Description { get; init; }

        public required ParameterDataType Type { get; init; }

        public required int Offset { get; init; }

        public required int Size { get; init; }

        public required byte[] BitMask { get; init; }

        public required string Correlator { get; init; }

        public required TelemetryUnit Unit { get; init; }

        public required double Min { get; init; }

        public required double Max { get; init; }
    }
}
