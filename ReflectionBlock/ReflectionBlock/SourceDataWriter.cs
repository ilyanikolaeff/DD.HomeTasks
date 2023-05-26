namespace ReflectionBlock
{
    internal class SourceDataWriter
    {
        public void WriteAllLines(string fileName, IEnumerable<string> lines)
        {
            File.WriteAllLines(fileName, lines);
        }
    }
}