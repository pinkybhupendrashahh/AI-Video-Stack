using AI_Video_Stack.Server.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AI_Video_Stack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IOllamaService _ollama; 
        private readonly ITtsService _tts; 
        private readonly IShotstackService _shotstack;
        private readonly StaticAssetsOptions _assets;
        public VideoController(IOllamaService ollama, ITtsService tts, IShotstackService shotstack, IOptions<StaticAssetsOptions> assets) {
            _ollama = ollama;
            _tts = tts; 
            _shotstack = shotstack; 
            _assets = assets.Value; 
        }
        [HttpPost("generate-script")]
        public async Task<IActionResult> GenerateScript([FromBody] ScriptRequest req)
        {
            var prompt = $"Write a {req.Style} narration about {req.Topic}. Keep it suitable for a {req.LengthSec} second voiceover.";
            var script = await _ollama.GenerateAsync(prompt);
            return Ok(new { script });
        }

        [HttpPost("render")]
        public async Task<IActionResult> Render([FromBody] RenderRequest req)
        {
            var script = string.IsNullOrWhiteSpace(req.Script)
                ? await _ollama.GenerateAsync($"Create a {req.Style} narration about {req.Topic} for ~{req.LengthSec} seconds.")
                : req.Script;
            var tts = await _tts.SynthesizeAsync(script, req.Voice);

            var audioUrl = tts.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? tts
                : $"{_assets.PublicOrigin}{tts}";

            var render = await _shotstack.RenderAsync(audioUrl, req.BackgroundVideoUrl, req.LengthSec, req.Title ?? "AI Narration");

            return Ok(new { jobId = render.id, status = render.status, script, audioUrl });
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(string id)
        {
            var s = await _shotstack.GetStatusAsync(id);
            return Ok(s);
        }
    }

    public record ScriptRequest(string Topic, string Style, double LengthSec);
    public record RenderRequest(string Topic, string Style, double LengthSec, string Voice, string BackgroundVideoUrl, string? Script, string? Title);
}
    

