using AI_Video_Stack.Server.Model;
using AI_Video_Stack.Server.Services;
using AI_Video_Stack.Server.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static System.Net.WebRequestMethods;

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
        private readonly GithubUploader _github;
        private readonly HttpClient _http;
        public VideoController(IHttpClientFactory factory, GithubUploader github, IOllamaService ollama, ITtsService tts, IShotstackService shotstack, IOptions<StaticAssetsOptions> assets , IConfiguration config) {
            _github = github;
            _ollama = ollama;
            _tts = tts; 
            _shotstack = shotstack; 
            _assets = assets.Value;
            _http = factory.CreateClient("Shotstack");
           // var ApiKey = config["Shotstack:ApiKey"]; // comes from user-secrets or env
           /// _http.DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }
        [HttpPost("generate-script")]
        public async Task<IActionResult> GenerateScript([FromBody] ScriptRequest req)
        {
            var prompt = $"Write a {req.Style} narration about {req.Topic}. Keep it suitable for a {req.LengthSec} second voiceover.";
            var script = await _ollama.GenerateAsync(prompt);
            return Ok(new { script });
        }

        //[HttpPost("render")]
        //public async Task<IActionResult> Render([FromBody] RenderRequest req)
        //{
        //    var script = string.IsNullOrWhiteSpace(req.Script)
        //        ? await _ollama.GenerateAsync($"Create a {req.Style} narration about {req.Topic} for ~{req.LengthSec} seconds.")
        //        : req.Script;
        //    var tts = await _tts.SynthesizeAsync(script, req.Voice);

        //    var audioUrl = tts.StartsWith("http", StringComparison.OrdinalIgnoreCase)
        //        ? tts
        //        : $"{_assets.PublicOrigin}{tts}";

        //    var render = await _shotstack.RenderAsync(audioUrl, req.BackgroundVideoUrl, req.LengthSec, req.Title ?? "AI Narration");

        //    return Ok(new { jobId = render.id, status = render.status, script, audioUrl });
        //}

        //[HttpPost("render")]
        //public async Task<IActionResult> Render([FromBody] RenderRequest req)
        //{ // 1) Generate/obtain audio // If audio arrives from FastAPI as a local path, use that; else synthesize here.
        //  var script = string.IsNullOrWhiteSpace(req.Script) ? req.Topic : req.Script;
        //    var localAudioPath = await _tts.SynthesizeAsync(script, req.Voice); //
        //                                                                              //returns local path // 2) Upload audio to GitHub Pages, get public URL
        //    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
        //    var audioFileName = $"audio-{timestamp}.mp3"; 
        //    var audioUrl = await _github.UploadAsync(localAudioPath, audioFileName); // 3) Background video: if user provided a public URL, use it; else skip
        //    var bgUrl = string.IsNullOrWhiteSpace(req.BackgroundVideoUrl) ? null : req.BackgroundVideoUrl; // 4) Render via Shotstack

        //    var render = await _shotstack.RenderAsync(audioUrl, bgUrl, req.LengthSec, req.Title ?? req.Topic);
        //    return Ok(render); }

        //    [HttpGet("status/{id}")]
        //public async Task<IActionResult> Status(string id)
        //{
        //    var s = await _shotstack.GetStatusAsync(id);
        //    return Ok(s);
        //}
        //[HttpGet("status/{id}")]
        //public async Task<IActionResult> Status(string id)
        //{
        //    var res = await _http.GetAsync($"https://api.shotstack.io/stage/render/{id}");

        //    if (!res.IsSuccessStatusCode)
        //    {
        //        var error = await res.Content.ReadAsStringAsync();
        //        return StatusCode((int)res.StatusCode, new { error });
        //    }

        //    var json = await res.Content.ReadAsStringAsync();
        //    // Pass through as JSON
        //    return Content(json, "application/json");
        //}

        //[HttpPost("render")]
        //public async Task<IActionResult> Render([FromBody] RenderRequest req)
        //{
        //    var script = string.IsNullOrWhiteSpace(req.Script)
        //        ? req.Topic
        //        : req.Script;

        //    var audioUrl = await _tts.SynthesizeAsync(script, req.Voice);

        //    var bgUrl = string.IsNullOrWhiteSpace(req.BackgroundVideoUrl)
        //        ? null
        //        : req.BackgroundVideoUrl;

        //    var renderRes = await _shotstack.RenderAsync(
        //        audioUrl,
        //        bgUrl,
        //        req.LengthSec,
        //        req.Title ?? req.Topic
        //    );

        //    return Ok(new
        //    {
        //        id = renderRes.Id,
        //        status = "queued"
        //    });
        //}


        //[HttpGet("status/{id}")]
        //public async Task<IActionResult> Status(string id)
        //{
        //    try
        //    {
        //        var res = await _http.GetAsync($"https://api.shotstack.io/stage/render/{id}");
        //        var body = await res.Content.ReadAsStringAsync();

        //        if (!res.IsSuccessStatusCode)
        //        {
        //            return StatusCode((int)res.StatusCode, new
        //            {
        //                success = false,
        //                error = body
        //            });
        //        }

        //        return Content(body, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            error = ex.Message
        //        });
        //    }
        //   }

        [HttpPost("render")]
        public async Task<IActionResult> Render([FromBody] RenderRequest req)
        {
            var script = string.IsNullOrWhiteSpace(req.Script) ? req.Topic : req.Script;

            // Step 1: Generate audio locally
            var localAudioPath = await _tts.SynthesizeAsync(script, req.Voice);

            // Step 2: Upload audio to GitHub Pages
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //var audioFileName = $"audio-{timestamp}.mp3";
            //var audioUrl = await _github.UploadAsync(localAudioPath, audioFileName);
         //   var localAudioPath = await _tts.SynthesizeAsync(script, req.Voice);

          //  var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var audioFileName = $"audio-{timestamp}.mp3";

            // Upload the local file, not the localhost URL
            var audioUrl = await _github.UploadAsync(localAudioPath, audioFileName);


            // Step 3: Background video
            var bgUrl = string.IsNullOrWhiteSpace(req.BackgroundVideoUrl) ? null : req.BackgroundVideoUrl;
            await Task.Delay(10000); // wait 10 seconds
            // Step 4: Render via Shotstack
            var renderRes = await _shotstack.RenderAsync(audioUrl, bgUrl, req.LengthSec, req.Title ?? req.Topic);

            return Ok(new { id = renderRes.Id, status = "queued" });
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(string id)
        {
            try
            {
                var res = await _http.GetAsync($"render/{id}");
                var body = await res.Content.ReadAsStringAsync();

                return Ok(JsonDocument.Parse(body).RootElement);
            }
            catch (Exception ex)
            {
                // ALWAYS JSON
                return Ok(new
                {
                    success = false,
                    error = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }


        //[HttpGet("status/{id}")]
        //public async Task<RenderResponse> Status(string id)
        //{
        //    var res = await _http.GetAsync($"https://api.shotstack.io/stage/render/{id}");

        //    // var res = await _http.GetAsync($"render/{id}");
        //    var json = await res.Content.ReadAsStringAsync();
        //    Console.WriteLine("Raw Shotstack JSON: " + json);
        //    var statusRes = JsonSerializer.Deserialize<ResponseWrapper>(json);
        //    return statusRes.Response;
        //}

    }

    public record ScriptRequest(string Topic, string Style, double LengthSec);
    public record RenderRequest(string Topic, string Style, double LengthSec, string Voice, string BackgroundVideoUrl, string? Script, string? Title);
}
    

