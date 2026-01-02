namespace AI_Video_Stack.Server.Services
{
    //    using AI_Video_Stack.Server.Services.Contracts;

    //    using Microsoft.Extensions.Options;
    //    using System.Net.Http.Json;


    //        public class ShotstackService : IShotstackService
    //        {
    //            private readonly HttpClient _http;
    //            private readonly ShotstackOptions _opt;
    //           // private string RenderBase => $"{_opt.BaseUrl}/{(_opt.Stage ? "stage" : "v1")}";

    //            public ShotstackService(IHttpClientFactory factory, IOptions<ShotstackOptions> opt)
    //            {
    //                _http = factory.CreateClient("Shotstack");
    //                _opt = opt.Value;
    //                _http.DefaultRequestHeaders.Add("x-api-key", _opt.ApiKey);
    //            // _http.BaseAddress = new Uri(RenderBase);
    //            _http.BaseAddress = new Uri(_opt.BaseUrl);
    //        }

    //            public async Task<RenderResponse> RenderAsync(string audioUrl, string backgroundUrl, double lengthSec, string title)
    //            {
    //                var payload = new
    //                {
    //                    timeline = new
    //                    {
    //                        tracks = new[]
    //                        {
    //                        new {
    //                            clips = new object[]
    //                            {
    //                                new { asset = new { type = "audio", src = audioUrl }, start = 0, length = lengthSec },
    //                                new { asset = new { type = "video", src = backgroundUrl }, start = 0, length = lengthSec },
    //                                new { asset = new { type = "title", text = title, style = "minimal" }, start = 0, length = Math.Min(5, lengthSec) }
    //                            }
    //                        }
    //                    }
    //                    },
    //                    output = new { format = "mp4", resolution = "hd" }
    //                };

    //                var res = await _http.PostAsJsonAsync("render", payload);
    //                res.EnsureSuccessStatusCode();
    //                var json = await res.Content.ReadFromJsonAsync<RenderResponse>();
    //                return json!;
    //            }

    //            public async Task<RenderStatus> GetStatusAsync(string id)
    //            {
    //                var res = await _http.GetAsync($"render/{id}");
    //                res.EnsureSuccessStatusCode();
    //                var json = await res.Content.ReadFromJsonAsync<RenderStatus>();
    //                return json!;
    //            }
    //        }
    //    }


    using AI_Video_Stack.Server.Services.Contracts;
    using Microsoft.Extensions.Options;

    public class ShotstackService : IShotstackService
    {
        private readonly HttpClient _http;
        private readonly ShotstackOptions _opt;

        public ShotstackService(IHttpClientFactory factory, IOptions<ShotstackOptions> opt)
        {
            _http = factory.CreateClient("Shotstack");
            _opt = opt.Value;

            // FIX: Use correct base URL directly from config
            // e.g. "https://api.shotstack.io/stage/" or "https://api.shotstack.io/production/"
            _http.BaseAddress = new Uri(_opt.BaseUrl);
            _http.DefaultRequestHeaders.Add("x-api-key", _opt.ApiKey);
        }

        public async Task<RenderResponse> RenderAsync(string audioUrl, string backgroundUrl, double lengthSec, string title)
        {
            // FIX: Build clips dynamically
            var clips = new List<object>
        {
            new { asset = new { type = "audio", src = audioUrl }, start = 0, length = lengthSec }
        };

            if (!string.IsNullOrWhiteSpace(backgroundUrl))
            {
                clips.Add(new { asset = new { type = "video", src = backgroundUrl }, start = 0, length = lengthSec });
            }

            clips.Add(new { asset = new { type = "title", text = title, style = "minimal" }, start = 0, length = Math.Min(5, lengthSec) });

            var payload = new
            {
                timeline = new { tracks = new[] { new { clips } } },
                output = new { format = "mp4", resolution = "hd" }
            };

            var res = await _http.PostAsJsonAsync("render", payload);
            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadFromJsonAsync<RenderResponse>();
            return json!;
        }

        public async Task<RenderStatus> GetStatusAsync(string id)
        {
            var res = await _http.GetAsync($"render/{id}");
            res.EnsureSuccessStatusCode();
            var json = await res.Content.ReadFromJsonAsync<RenderStatus>();
            return json!;
        }
    }
}
