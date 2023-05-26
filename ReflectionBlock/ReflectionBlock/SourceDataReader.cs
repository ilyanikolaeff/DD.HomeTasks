namespace ReflectionBlock
{
    internal class SourceDataReader
    {
        public IEnumerable<string> ReadAllLines(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            return lines;
        }
    }
}