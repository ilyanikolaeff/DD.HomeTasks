namespace ReflectionBlock.Library
{
    public class TextProcessor
    {
        private IEnumerable<string> ProcessTextLines(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                yield return ProcessTextLine(line);
            }
        }


        private string ProcessTextLine(string line)
        {
            var newLine = line.Trim();
            newLine = newLine.ToUpper();
            newLine = new string(newLine.Reverse().ToArray());
            return newLine;
        }

        public IEnumerable<string> ProcessTextLinesParallel(IEnumerable<string> lines)
        {
            var resultLines = lines.AsParallel().Select(s => ProcessTextLine(s));
            return resultLines;
        }

        public IEnumerable<string> ProcessTextLinesSync(IEnumerable<string> lines)
        {
            return ProcessTextLines(lines);
        }


        public IEnumerable<KeyValuePair<string, int>> ProcessTextToDictionary(IEnumerable<string> lines)
        {
            foreach (var line in lines) 
            {
                yield return new KeyValuePair<string, int>(line, line.Length);
            }
        }
    }
}