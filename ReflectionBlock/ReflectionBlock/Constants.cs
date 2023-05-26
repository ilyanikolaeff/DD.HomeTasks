namespace ReflectionBlock
{
    internal static class Constants
    {
        public const string InputFileName = "input.txt";
        public const string OutputFileName = "output.txt";

        public const string ClassName = "TextProcessor";
        public const string MethodName = "ProcessTextLines";

        public const string AssemblyName = "ReflectionBlock.Library.dll";
        public static readonly string AbsoluteAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AssemblyName);
    }
}