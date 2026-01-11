namespace AI_Video_Stack.Server.Services
{
    using AI_Video_Stack.Server.Services.Contracts;
    //    using AI_Video_Stack.Server.Services.Contracts;

    //    using Microsoft.Extensions.Options;
    //    using System.Net.Http.Json;


    //        public class TtsService : ITtsService
    //        {
    //            private readonly HttpClient _http;

    //            public TtsService(IHttpClientFactory factory, IOptions<TtsServiceOptions> opt)
    //            {
    //                _http = factory.CreateClient("TtsService");
    //                _http.BaseAddress = new Uri(opt.Value.BaseUrl);
    //            }

    //        //public async Task<TtsResult> SynthesizeAsync(string text, string voice = "default")
    //        //{
    //        //    var res = await _http.PostAsJsonAsync("/tts", new { text, voice });
    //        //    res.EnsureSuccessStatusCode();
    //        //    var json = await res.Content.ReadFromJsonAsync<TtsResult>();
    //        //    return json!;
    //        //}
    //        //public async Task<TtsResult> SynthesizeAsync(string text, string voice)
    //        //{
    //        //    var req = new { text, voice };
    //        //    var res = await _http.PostAsJsonAsync("narrate", req);
    //        //    res.EnsureSuccessStatusCode();

    //        //    var json = await res.Content.ReadFromJsonAsync<TtsResult>();
    //        //    return json ?? new TtsResult();
    //        //}

    //        //public async Task<string> SynthesizeAsync(string text, string voice)
    //        //{
    //        //    var req = new { text, voice };
    //        //    var res = await _http.PostAsJsonAsync("narrate", req);
    //        //    res.EnsureSuccessStatusCode();
    //        //    var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}.mp3";
    //        //    var audioBytes = await res.Content.ReadAsByteArrayAsync();


    //        //    //using var file = File.Create(dynamicName);
    //        //    //await stream.CopyToAsync(file);

    //        //    await File.WriteAllBytesAsync(fileName, audioBytes);

    //        //    // validation
    //        //    if (new FileInfo(fileName).Length < 50_000)
    //        //        throw new Exception("Invalid audio generated");

    //        //    return fileName;
    //        //}

    //        //public async Task<string> SynthesizeAsync(string text, string voice)
    //        //{
    //        //    var res = await _http.PostAsJsonAsync("narrate", new { text, voice });
    //        //    res.EnsureSuccessStatusCode();
    //        //    var contentType = res.Content.Headers.ContentType?.ToString();
    //        //    var body = await res.Content.ReadAsStringAsync();

    //        //    throw new Exception($"TTS DEBUG → Content-Type: {contentType}, Body: {body}");

    //        //    var tts = await res.Content.ReadFromJsonAsync<TtsResponse>();
    //        //    if (tts == null || string.IsNullOrWhiteSpace(tts.Url))
    //        //        throw new Exception("TTS API did not return audio URL");

    //        //    // Download actual audio
    //        //    var audioBytes = await _http.GetByteArrayAsync(tts.Url);

    //        //    var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}.mp3";
    //        //    await File.WriteAllBytesAsync(fileName, audioBytes);

    //        //    if (new FileInfo(fileName).Length < 50_000)
    //        //        throw new Exception("Downloaded audio is invalid");

    //        //    return fileName;
    //        //}
    //        public async Task<string> SynthesizeAsync(string text, string voice)
    //        {
    //            var res = await _http.PostAsJsonAsync("narrate", new { text, voice });
    //            res.EnsureSuccessStatusCode();

    //            var tts = await res.Content.ReadFromJsonAsync<TtsResponse>();
    //            if (tts == null || string.IsNullOrWhiteSpace(tts.PublicUrl))
    //                throw new Exception("TTS API did not return audio URL");

    //            var audioUrl = tts.PublicUrl.StartsWith("http")
    //                ? tts.PublicUrl
    //                : new Uri(_http.BaseAddress!, tts.PublicUrl).ToString();

    //            var audioBytes = await _http.GetByteArrayAsync(audioUrl);

    //            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}.mp3";
    //            await File.WriteAllBytesAsync(fileName, audioBytes);

    //            if (new FileInfo(fileName).Length < 50_000)
    //                throw new Exception("Downloaded audio is invalid");

    //            return fileName;
    //        }


    //    }
    //    public class TtsResponse
    //    {
    //        public string PublicUrl { get; set; } = "";
    //    }

    //}


    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Net.Http;
    using System.Threading.Tasks;
    using static System.Net.WebRequestMethods;

    public class TtsService : ITtsService
    {
        private readonly HttpClient _http;

        public TtsService(IHttpClientFactory factory, IOptions<TtsServiceOptions> opt)
        {
            _http = factory.CreateClient("TtsService");
            _http.BaseAddress = new Uri(opt.Value.BaseUrl); // http://localhost:8000
        }
        public async Task<string> SynthesizeAsync(string text, string? voice)
        {
            var payload = new { text, voice };
            var res = await _http.PostAsJsonAsync("tts", payload);
            res.EnsureSuccessStatusCode();

            var tts = await res.Content.ReadFromJsonAsync<TtsResult>();
            if (tts == null || string.IsNullOrWhiteSpace(tts.PublicUrl))
                throw new Exception("TTS did not return fileName");

            // Return local path for uploader
           // var localPath = tts.PublicUrl;
           var localPath = Path.Combine("C:\\Users\\Samsung\\source\\repos\\AI-Video-Stack\\AI-Video-Stack.Server", tts.PublicUrl);
            return localPath;
        }


        //public async Task<string> SynthesizeAsync(string text, string? voice)
        //{
        //    var payload = new
        //    {
        //        text,
        //        voice
        //    };

        //    var res = await _http.PostAsJsonAsync("tts", payload);

        //  //  Console.WriteLine("TTS DEBUG → BaseAddress: {base}, RequestUri: {uri}, Status: {status}", _http.BaseAddress,  res.RequestMessage?.RequestUri,res.StatusCode);
        //    res.EnsureSuccessStatusCode();

        //    var tts = await res.Content.ReadFromJsonAsync<TtsResult>();
        //    if (tts == null || string.IsNullOrWhiteSpace(tts.PublicUrl))
        //        throw new Exception("TTS did not return publicUrl");

        //    // Convert /assets/... → absolute URL
        //    var absoluteUrl = new Uri(_http.BaseAddress!, tts.PublicUrl).ToString();


        //    return absoluteUrl;
        //}
    }
}

//    public class TtsService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<TtsService> _logger;
//        private readonly string _ttsBaseUrl = "https://your-tts-api.com/synthesize"; // replace with your TTS API base URL
//        private readonly string[] _validVoices = new[] { "male", "female", "neutral" }; // example voices

//        public TtsService(IHttpClientFactory factory, ILogger<TtsService> logger, IOptions<TtsServiceOptions> opt)
//        {

//            _logger = logger;
//            _httpClient = factory.CreateClient("TtsService");
//             _httpClient.BaseAddress = new Uri(opt.Value.BaseUrl);
//        }

//        public async Task<byte[]> SynthesizeAsync(string text, string voice)
//        {
//            if (string.IsNullOrWhiteSpace(text))
//                throw new ArgumentException("Text cannot be empty", nameof(text));

//            if (string.IsNullOrWhiteSpace(voice) || !_validVoices.Contains(voice.ToLower()))
//                throw new ArgumentException($"Invalid voice. Valid options are: {string.Join(", ", _validVoices)}", nameof(voice));

//            try
//            {
//                // URL encode the text
//                var urlEncodedText = Uri.EscapeDataString(text);
//                var requestUrl = $"{_ttsBaseUrl}?voice={voice}&text={urlEncodedText}";

//                _logger.LogInformation($"Calling TTS API: {requestUrl}");

//                var audioBytes = await _httpClient.GetByteArrayAsync(requestUrl);
//                return audioBytes;
//            }
//            catch (HttpRequestException ex)
//            {
//                _logger.LogError($"TTS API call failed. Exception: {ex.Message}");
//                throw new Exception($"Failed to call TTS API. Check your URL and voice parameter. See inner exception for details.", ex);
//            }
//        }
//    }
//}