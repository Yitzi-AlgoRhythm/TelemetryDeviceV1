using System.Text.Json;

namespace TelemetryDeviceV1.ICD
{
    public class IcdDeserializer
    {
        public required Dictionary<byte, IcdParameter[]> CorrelatorGroups { get; init; }

        private static readonly string filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "ICD_parameters.json");

        private static readonly string[] groupIdentifiers = ["0.25", "1.0", "2.0", "4.0", "8.0", "16.0"];
        private static readonly byte[] byteIdentifiers = [0, 1, 2, 4, 8, 16];


        public IcdDeserializer()
        {
            IcdParameter[] icdParameters;

            using (FileStream stream = File.OpenRead(filePath))
            {
                icdParameters = JsonSerializer.Deserialize<IcdParameter[]>(stream)!; // check if there's a better way to deal with null here
            }

            CorrelatorGroups = new Dictionary<byte, IcdParameter[]>();

            for (int i = 0; i < groupIdentifiers.Length; i++)
            {
                CorrelatorGroups.Add(byteIdentifiers[i], icdParameters.Where(param => param.Correlator == groupIdentifiers[i]).ToArray());
            }
        }
    }
}
