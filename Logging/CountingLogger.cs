namespace TelemetryDeviceV1.Logging
{
    public class CountingLogger : ILoggerTD
    {
        private readonly Dictionary<string, int> messageCounts;

        public CountingLogger()
        {
            messageCounts = new Dictionary<string, int>();
        }

        public void Log(string message)
        {
            messageCounts.TryAdd(message, 1);
            messageCounts[message]++;

            Console.WriteLine($"{message} - {messageCounts[message]}");
        }
    }
}
