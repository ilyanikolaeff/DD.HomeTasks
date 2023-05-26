using ReflectionBlock.Library;
using System;
using System.Diagnostics;

namespace ReflectionBlock
{
    internal class EntryPoint
    {
        private readonly AssemblyHelper _assemblyHelper;
        private readonly SourceDataReader _reader;
        private readonly SourceDataWriter _writer;
        private readonly ConsoleLogger _logger;

        public EntryPoint(AssemblyHelper assemblyHelper, SourceDataReader reader, SourceDataWriter writer, ConsoleLogger logger)
        {
            _assemblyHelper = assemblyHelper ?? throw new ArgumentNullException(nameof(assemblyHelper));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RunTaskOne()
        {
            _logger.LogInfo("Running task one");
            var assembly = _assemblyHelper.LoadAssembly(Constants.AbsoluteAssemblyPath);
            var typeInfo = _assemblyHelper.GetTypeInfo(assembly, Constants.ClassName);
            var methodInfo = _assemblyHelper.GetMethodInfo(typeInfo, Constants.MethodName);
            var lines = _reader.ReadAllLines(Constants.InputFileName);
            var resultLines = (IEnumerable<string>)_assemblyHelper.CallMethod(typeInfo, methodInfo, new object[] { lines });
            _writer.WriteAllLines(Constants.OutputFileName, resultLines);
            _logger.LogInfo("Task one finished");
        }

        public void RunTaskTwo()
        {
            _logger.LogInfo("Runnig task two");

            var textProcessor = new TextProcessor();

            var generator = new RandomStringGenerator();
            var lines = generator.GenerateRandomStrings(100_000, 100);

            _logger.LogInfo("Starting sync processing lines");
            var watcher = Stopwatch.StartNew();
            var resultLines = textProcessor.ProcessTextLinesSync(lines);
            _logger.LogInfo($"Sync elapsed: {watcher.Elapsed}");
            watcher.Stop();

            watcher.Restart();
            _logger.LogInfo("Starting parallel processing");
            resultLines = textProcessor.ProcessTextLinesParallel(lines);
            watcher.Stop();
            _logger.LogInfo($"Parallel elapsed: {watcher.Elapsed}");

            _writer.WriteAllLines(Constants.OutputFileName, resultLines);

            _logger.LogInfo("Task two finished");
        }



    }

    internal class RandomStringGenerator
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random random = new Random();
        public string GenerateRandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IEnumerable<string> GenerateRandomStrings(int count, int length) 
        {
            for(int i =0; i < count; i++) 
            {
                yield return GenerateRandomString(length);
            }
        }
    }
}