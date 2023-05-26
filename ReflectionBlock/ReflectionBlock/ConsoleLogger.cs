namespace ReflectionBlock
{
    internal class ConsoleLogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] | {message}");
        }
    }
}