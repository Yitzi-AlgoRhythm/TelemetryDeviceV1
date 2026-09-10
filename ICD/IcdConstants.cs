namespace TelemetryDeviceV1.Deserialization
{
    public static class IcdConstants
    {
        public static readonly byte[] SyncBytes = { 66, 32, 23 };

        public static readonly string IcdFile = Path.Combine(AppContext.BaseDirectory, "Resources", "ICD_parameters.json");
    }
}
