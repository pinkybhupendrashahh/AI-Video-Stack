namespace AI_Video_Stack.Server.Services
{
    using AI_Video_Stack.Server.Services.Contracts;
  
    using Microsoft.Extensions.Options;
    using System.Net.Http.Json;

   
        public class TtsService : ITtsService
        {
            private readonly HttpClient _http;

            public TtsService(IHttpClientFactory factory, IOptions<TtsServiceOptions> opt)
            {
                _http = factory.CreateClient("TtsService");
                _http.BaseAddress = new Uri(opt.Value.BaseUrl);
            }

        //public async Task<TtsResult> SynthesizeAsync(string text, string voice = "default")
        //{
        //    var res = await _http.PostAsJsonAsync("/tts", new { text, voice });
        //    res.EnsureSuccessStatusCode();
        //    var json = await res.Content.ReadFromJsonAsync<TtsResult>();
        //    return json!;
        //}
        //public async Task<TtsResult> SynthesizeAsync(string text, string voice)
        //{
        //    var req = new { text, voice };
        //    var res = await _http.PostAsJsonAsync("narrate", req);
        //    res.EnsureSuccessStatusCode();

        //    var json = await res.Content.ReadFromJsonAsync<TtsResult>();
        //    return json ?? new TtsResult();
        //}

        public async Task<string> SynthesizeAsync(string text, string voice)
        {
            var req = new { text, voice };
            var res = await _http.PostAsJsonAsync("narrate", req);
            res.EnsureSuccessStatusCode();

            using var stream = await res.Content.ReadAsStreamAsync();
            var dynamicName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}.mp4";

            using var file = File.Create(dynamicName);
            await stream.CopyToAsync(file);

            return dynamicName;
        }




    }
    public class TtsResponse
    {
        public string Url { get; set; } = "";
    }

}


