using Microsoft.AspNetCore.Mvc;
using ReflectionBlock.Library;

namespace ReflectionBlock.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly TextProcessor _textProcessor;
        private readonly ILogger<MainController> _logger;

        public MainController(TextProcessor textProcessor, ILogger<MainController> logger)
        {
            _logger = logger;
            _textProcessor = textProcessor;
        }

        [HttpGet(Name = "GetDictionary")]
        public IEnumerable<KeyValuePair<string, int>> Get(string text)
        {
            return _textProcessor.ProcessTextToDictionary(text);
        }
    }
}