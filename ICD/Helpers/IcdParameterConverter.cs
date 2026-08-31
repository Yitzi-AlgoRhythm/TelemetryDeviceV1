namespace TelemetryDeviceV1.ICD.Helpers
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using global::TelemetryDeviceV1.ICD.Enums;
    using ParameterDataLib;

    namespace TelemetryDeviceV1.Deserialization
    {
        public class IcdParameterConverter : JsonConverter<IcdParameter>
        {
            public override IcdParameter Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                using JsonDocument doc = JsonDocument.ParseValue(ref reader);
                JsonElement root = doc.RootElement;

                string name = root.GetProperty("Name").GetString()!;

                string description = root.GetProperty("Description").GetString()!;

                ParameterDataType type = JsonSerializer.Deserialize<ParameterDataType>(root.GetProperty("Type").GetRawText(), options);

                int offset = root.GetProperty("Offset").GetInt32();

                int size = root.GetProperty("Size").GetInt32();

                string correlator = root.GetProperty("Correlator").GetString()!;

                TelemetryUnit unit = JsonSerializer.Deserialize<TelemetryUnit>(root.GetProperty("Unit").GetRawText(), options);

                double min = root.GetProperty("Min").GetDouble();

                double max = root.GetProperty("Max").GetDouble();

                string bitMaskRaw = root.GetProperty("BitMask").GetString()!;

                byte[] bitMask = Convert.FromHexString(bitMaskRaw[2..]);

                return new IcdParameter
                {
                    Name = name,
                    Description = description,
                    Type = type,
                    Offset = offset,
                    Size = size,
                    BitMask = bitMask,
                    Correlator = correlator,
                    Unit = unit,
                    Min = min,
                    Max = max
                };
            }

            public override void Write(
                Utf8JsonWriter writer,
                IcdParameter value,
                JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
