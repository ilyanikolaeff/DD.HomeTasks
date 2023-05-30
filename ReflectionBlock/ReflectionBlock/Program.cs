namespace ReflectionBlock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting application...");

            var entryPoing = new EntryPoint(
                new AssemblyHelper(),
                new SourceDataReader(),
                new SourceDataWriter(),
                new ConsoleLogger());

            entryPoing.RunTaskOne();

            entryPoing.RunTaskTwo();

            entryPoing.RunTaskThree();


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}