using System.Text.Json;

namespace TelemetryDeviceV1.ICD
{
    public class IcdDeserializer
    {
        public required Dictionary<string, IcdParameter[]> CorrelatorGroups { get; init; }

        private static readonly string filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "ICD_parameters.json");

        private static readonly string[] groupIdentifiers = {"0.25", "1.0",  "2.0", "4.0", "8.0", "16.0"};


        public IcdDeserializer()
        {
            IcdParameter[] icdParameters;

            using (FileStream stream = File.OpenRead(filePath))
            {
                icdParameters = JsonSerializer.Deserialize<IcdParameter[]>(stream)!; // check if there's a better way to deal with null here
            }

            CorrelatorGroups = new Dictionary<string, IcdParameter[]>();

            foreach (string identifier in groupIdentifiers)
            {
                CorrelatorGroups.Add(identifier, icdParameters.Where(param => param.Correlator == identifier).ToArray());
            }
        }
    }
}
