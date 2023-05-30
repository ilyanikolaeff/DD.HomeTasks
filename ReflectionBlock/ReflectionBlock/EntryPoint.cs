using ReflectionBlock.Library;
using ReflectionBlock.WebService.Models;
using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

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

            var comparer = new SpeedComparer();
            var textProcessor = new TextProcessor();

            var generator = new RandomStringGenerator();
            var lines = generator.GenerateRandomStrings(100_000, 100);

            _logger.LogInfo("Starting sync processing lines");
            var watcher = Stopwatch.StartNew();
            var resultLines = textProcessor.ProcessTextLinesSync(lines);
            _logger.LogInfo($"Sync elapsed: {watcher.Elapsed}");
            watcher.Stop();
            comparer.AddToCompare("SYNC", watcher.Elapsed);

            watcher.Restart();
            _logger.LogInfo("Starting parallel processing");
            resultLines = textProcessor.ProcessTextLinesParallel(lines);
            watcher.Stop();
            _logger.LogInfo($"Parallel elapsed: {watcher.Elapsed}");
            comparer.AddToCompare("PARALLEL", watcher.Elapsed);
            _writer.WriteAllLines(Constants.OutputFileName, resultLines);

            comparer.PrintResults(_logger.LogInfo);

            _logger.LogInfo("Task two finished");
        }


        public void RunTaskThree()
        {
            Thread.Sleep(5000);

            _logger.LogInfo("Running task three");
            var stringGenerator = new RandomStringGenerator();
            var lines = stringGenerator.GenerateRandomStrings(100, 100);

            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:7001/main");
            requestMessage.Content = JsonContent.Create(new GetDictionaryRequest() { Lines = lines });

            var responseMessage = httpClient.Send(requestMessage);
            var result = Task.Run(async () => await responseMessage.Content.ReadFromJsonAsync<GetDictionaryResponse>()).GetAwaiter().GetResult();
            _logger.LogInfo($"Printing results of http request{Environment.NewLine}{string.Join(Environment.NewLine, result.Result.Select(s => $"{s.Key} - {s.Value}"))}");

            _logger.LogInfo("Task three finished");
        }

    }

    internal class SpeedComparer
    {
        private Dictionary<string, TimeSpan> _comparedSpeeds = new Dictionary<string, TimeSpan>();

        public void AddToCompare(string name, TimeSpan elapsedTime)
        {
            _comparedSpeeds[name] = elapsedTime;
        }

        public void PrintResults(Action<string> output)
        {
            var results = new StringBuilder();
            results.AppendLine("Printing speed comparsion");
            int index = 1;
            foreach (var compareVal in _comparedSpeeds)
            {
                results.AppendLine($"[{index++}] {compareVal.Key} - {compareVal.Value}");
            }

            var max = _comparedSpeeds.MaxBy(s => s.Value);
            var min = _comparedSpeeds.MinBy(s => s.Value);
            results.Append($"Max - {max.Key} ({max.Value}), min - {min.Key} ({min.Value})");

            output(results.ToString());
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