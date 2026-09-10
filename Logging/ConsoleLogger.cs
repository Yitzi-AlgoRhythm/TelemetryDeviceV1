namespace TelemetryDeviceV1.Logging
{
    public class ConsoleLogger : ILoggerTD
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
