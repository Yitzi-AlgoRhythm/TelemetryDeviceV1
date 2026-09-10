using System.Security.Cryptography;
using System.Text.Json;
using TelemetryDeviceV1.Deserialization;

namespace TelemetryDeviceV1.ICD
{
    public class IcdDeserializer
    {
        public required Dictionary<byte, List<IcdParameter>> CorrelatorGroups { get; init; }


        public IcdDeserializer()
        {
            List<IcdParameter> icdParameters;

            using (FileStream stream = File.OpenRead(IcdConstants.IcdFile))
            {
                icdParameters = JsonSerializer.Deserialize<List<IcdParameter>>(stream)!;
            }

            CorrelatorGroups = [];

            foreach (IcdParameter parameter in icdParameters)
            {
                byte paramCorrelator = (byte)parameter.Correlator;

                if (!CorrelatorGroups.ContainsKey(paramCorrelator))
                {
                    CorrelatorGroups.Add(paramCorrelator, []);
                }

                CorrelatorGroups[paramCorrelator].Add(parameter);
            }
        }
    }
}
