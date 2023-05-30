using Microsoft.AspNetCore.Mvc;
using ReflectionBlock.Library;
using ReflectionBlock.WebService.Models;

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
        public GetDictionaryResponse Get([FromBody]GetDictionaryRequest request)
        {
            var result = new GetDictionaryResponse() { Result = _textProcessor.ProcessTextToDictionary(request.Lines) };
            return result;
        }
    }
}