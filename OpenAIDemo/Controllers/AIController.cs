using Microsoft.AspNetCore.Mvc;
using OpenAIDemo.Service;

namespace OpenAIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly GeminiService _geminiService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AIController(
            GeminiService geminiService,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _geminiService = geminiService;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(QuestionRequest request)
        {
            var response =
                await _geminiService.AskQuestion(request.Question);

            return Ok(response);
        }

        [HttpGet("models")]
        public async Task<IActionResult> GetModels()
        {
            var apiKey = _configuration["Gemini:ApiKey"];

            var response = await _httpClient.GetAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}");

            var result = await response.Content.ReadAsStringAsync();

            return Content(result, "application/json");
        }
    }
}