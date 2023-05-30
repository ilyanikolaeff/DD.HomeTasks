namespace ReflectionBlock.WebService.Models
{
    public class GetDictionaryRequest
    {
        public IEnumerable<string> Lines { get; set; }
    }

    public class GetDictionaryResponse
    {
        public IEnumerable<KeyValuePair<string, int>> Result { get; set; }
    }
}
