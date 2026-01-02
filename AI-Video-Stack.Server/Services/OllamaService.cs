namespace AI_Video_Stack.Server.Services
{
    using AI_Video_Stack.Server.Services.Contracts;
 
    using Microsoft.Extensions.Options;
    using System.Net.Http.Json;

    
        public class OllamaService : IOllamaService
        {
            private readonly HttpClient _http;
            private readonly OllamaOptions _opt;

            public OllamaService(IHttpClientFactory factory, IOptions<OllamaOptions> opt)
            {
                _http = factory.CreateClient("Ollama");
                _opt = opt.Value;
                _http.BaseAddress = new Uri(_opt.BaseUrl);
            }

        //public async Task<string> GenerateAsync(string prompt)
        //{
        //    var req = new { model = _opt.Model, prompt, stream = false };

        //    var res = await _http.PostAsJsonAsync("api/generate", req);
        //Console.WriteLine($"Response status: {res.StatusCode}");
        //var raw = res.EnsureSuccessStatusCode();
        //Console.WriteLine($"Raw response: {raw}");
        //var json = await res.Content.ReadFromJsonAsync<OllamaResponse>();
        //    return json?.Response ?? "";
        //}
        public async Task<string> GenerateAsync(string prompt)
        {
            var req = new { model = _opt.Model, prompt, stream = false };
            var res = await _http.PostAsJsonAsync("api/generate", req);
            res.EnsureSuccessStatusCode();

            var raw = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"Ollama raw response: {raw}");

            using var doc = System.Text.Json.JsonDocument.Parse(raw);
            if (doc.RootElement.TryGetProperty("response", out var resp))
            {
                return resp.GetString() ?? "";
            }
            return "";
        }

        private class OllamaResponse { public string Response { get; set; } = ""; }
        }
    }


