using System.Text.Json.Serialization;

namespace TelemetryDeviceV1.ICD.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ParameterDataType
    {
        [JsonStringEnumMemberName("uint1")]
        Uint1,
        [JsonStringEnumMemberName("uint2")]
        Uint2,
        [JsonStringEnumMemberName("uint3")]
        Uint3,
        [JsonStringEnumMemberName("uint4")]
        Uint4,
        [JsonStringEnumMemberName("uint8")]
        Uint8,
        [JsonStringEnumMemberName("uint16")]
        Uint16,
        [JsonStringEnumMemberName("int16")]
        Int16,
        [JsonStringEnumMemberName("int32")]
        Int32,
        [JsonStringEnumMemberName("float64")]
        Float64
    }
}
