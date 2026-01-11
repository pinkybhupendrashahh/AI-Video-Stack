namespace AI_Video_Stack.Server.Services
{
    using AI_Video_Stack.Server.Model;
    using AI_Video_Stack.Server.Services.Contracts;
    using Microsoft.Extensions.Options;
 
    using System.Net.Http.Headers;
    //    //    using AI_Video_Stack.Server.Services.Contracts;

    //    //    using Microsoft.Extensions.Options;
    //    //    using System.Net.Http.Json;


    //    //        public class ShotstackService : IShotstackService
    //    //        {
    //    //            private readonly HttpClient _http;
    //    //            private readonly ShotstackOptions _opt;
    //    //           // private string RenderBase => $"{_opt.BaseUrl}/{(_opt.Stage ? "stage" : "v1")}";

    //    //            public ShotstackService(IHttpClientFactory factory, IOptions<ShotstackOptions> opt)
    //    //            {
    //    //                _http = factory.CreateClient("Shotstack");
    //    //                _opt = opt.Value;
    //    //                _http.DefaultRequestHeaders.Add("x-api-key", _opt.ApiKey);
    //    //            // _http.BaseAddress = new Uri(RenderBase);
    //    //            _http.BaseAddress = new Uri(_opt.BaseUrl);
    //    //        }

    //    //            public async Task<RenderResponse> RenderAsync(string audioUrl, string backgroundUrl, double lengthSec, string title)
    //    //            {
    //    //                var payload = new
    //    //                {
    //    //                    timeline = new
    //    //                    {
    //    //                        tracks = new[]
    //    //                        {
    //    //                        new {
    //    //                            clips = new object[]
    //    //                            {
    //    //                                new { asset = new { type = "audio", src = audioUrl }, start = 0, length = lengthSec },
    //    //                                new { asset = new { type = "video", src = backgroundUrl }, start = 0, length = lengthSec },
    //    //                                new { asset = new { type = "title", text = title, style = "minimal" }, start = 0, length = Math.Min(5, lengthSec) }
    //    //                            }
    //    //                        }
    //    //                    }
    //    //                    },
    //    //                    output = new { format = "mp4", resolution = "hd" }
    //    //                };

    //    //                var res = await _http.PostAsJsonAsync("render", payload);
    //    //                res.EnsureSuccessStatusCode();
    //    //                var json = await res.Content.ReadFromJsonAsync<RenderResponse>();
    //    //                return json!;
    //    //            }

    //    //            public async Task<RenderStatus> GetStatusAsync(string id)
    //    //            {
    //    //                var res = await _http.GetAsync($"render/{id}");
    //    //                res.EnsureSuccessStatusCode();
    //    //                var json = await res.Content.ReadFromJsonAsync<RenderStatus>();
    //    //                return json!;
    //    //            }
    //    //        }
    //    //    }


    //    using AI_Video_Stack.Server.Services.Contracts;
    //    using Microsoft.Extensions.Options;

    //    public class ShotstackService : IShotstackService
    //    {
    //        private readonly HttpClient _http;
    //        private readonly ShotstackOptions _opt;

    //        public ShotstackService(IHttpClientFactory factory, IOptions<ShotstackOptions> opt)
    //        {
    //            _http = factory.CreateClient("Shotstack");
    //            _opt = opt.Value;

    //            // FIX: Use correct base URL directly from config
    //            // e.g. "https://api.shotstack.io/stage/" or "https://api.shotstack.io/production/"
    //            _http.BaseAddress = new Uri(_opt.BaseUrl);
    //            _http.DefaultRequestHeaders.Add("x-api-key", _opt.ApiKey);
    //        }

    //        public async Task<RenderResponse> RenderAsync(string audioUrl, string backgroundUrl, double lengthSec, string title)
    //        {
    //            // FIX: Build clips dynamically
    //            var clips = new List<object>
    //        {
    //            new { asset = new { type = "audio", src = audioUrl }, start = 0, length = lengthSec }
    //        };

    //            if (!string.IsNullOrWhiteSpace(backgroundUrl))
    //            {
    //                clips.Add(new { asset = new { type = "video", src = backgroundUrl }, start = 0, length = lengthSec });
    //            }

    //            clips.Add(new { asset = new { type = "title", text = title, style = "minimal" }, start = 0, length = Math.Min(5, lengthSec) });

    //            var payload = new
    //            {
    //                timeline = new { tracks = new[] { new { clips } } },
    //                output = new { format = "mp4", resolution = "hd" }
    //            };

    //            var res = await _http.PostAsJsonAsync("render", payload);
    //            res.EnsureSuccessStatusCode();

    //            var json = await res.Content.ReadFromJsonAsync<RenderResponse>();
    //            return json!;
    //        }

    //        public async Task<RenderStatus> GetStatusAsync(string id)
    //        {
    //            var res = await _http.GetAsync($"render/{id}");
    //            res.EnsureSuccessStatusCode();
    //            var json = await res.Content.ReadFromJsonAsync<RenderStatus>();
    //            return json!;
    //        }
    //    }
    //}
    // Services/ShotstackService.cs
        using System.Net.Http.Json;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class ShotstackService : IShotstackService
        {
            private readonly HttpClient _http;
            private readonly ShotstackOptions _opt;

            public ShotstackService(IHttpClientFactory factory, IOptions<ShotstackOptions> opt , IConfiguration config)
            {
                _http = factory.CreateClient("Shotstack");
                _opt = opt.Value;
            //    _http.BaseAddress = new Uri(_opt.BaseUrl);          // e.g. https://api.shotstack.io/stage/
            //    //_http.DefaultRequestHeaders.Add("x-api-key", _opt.ApiKey);
            //     var ApiKey = config["Shotstack:ApiKey"]; // comes from user-secrets or env
            //_http.DefaultRequestHeaders.Accept.Clear();
            //_http.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json")
            //);

            //_http.DefaultRequestHeaders.Remove("x-api-key");
            //_http.DefaultRequestHeaders.Add("x-api-key", ApiKey);
            //// _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
            //Console.WriteLine($"Loaded Shotstack API key: {ApiKey}");
        }

        //public async Task<Model.RenderResponse> RenderAsync(string audioUrl, string? backgroundUrl, double lengthSec, string title)
        //{
        //    var clips = new List<object>
        //    {
        //        new { asset = new { type = "audio", src = audioUrl }, start = 0, length = lengthSec }
        //    };

        //    if (!string.IsNullOrWhiteSpace(backgroundUrl))
        //    {
        //        clips.Add(new { asset = new { type = "video", src = backgroundUrl }, start = 0, length = lengthSec });
        //    }

        //    clips.Add(new
        //    {
        //        asset = new { type = "title", text = title, style = "minimal", position = "bottom" },
        //        start = 0,
        //        length = Math.Min(5, lengthSec)
        //    });

        //    var payload = new
        //    {
        //        timeline = new { tracks = new[] { new { clips } } },
        //        output = new { format = "mp4", resolution = "hd" }
        //    };
        //    // Render
        //    //var res = await _http.PostAsJsonAsync("render", payload);
        //    //var json = await res.Content.ReadAsStringAsync();
        //    //var renderRes = JsonSerializer.Deserialize<StatusResponseWrapper>(json);

        //    //if (renderRes?.Response == null)
        //    //    throw new InvalidOperationException("Shotstack response or its 'Response' property was null.");
        //    var res = await _http.PostAsJsonAsync("render", payload);

        //    if (!res.IsSuccessStatusCode)
        //    {
        //        var errorText = await res.Content.ReadAsStringAsync();
        //        throw new InvalidOperationException($"Shotstack API error: {res.StatusCode} - {errorText}");
        //    }

        //    var shotstackResponse = await res.Content.ReadFromJsonAsync<ShotstackResponse>();

        //    if (shotstackResponse?.Response == null)
        //    {
        //        throw new InvalidOperationException("Shotstack response or its 'Response' property was null.");
        //    }

        //    return shotstackResponse.Response;


        //    // Status
        //    var res1 = await _http.GetAsync($"render/{renderRes.Response.Id}");
        //    var json1 = await res1.Content.ReadAsStringAsync();
        //    var statusRes = JsonSerializer.Deserialize<StatusResponseWrapper>(json1);

        //    if (statusRes?.Response == null)
        //        throw new InvalidOperationException("Shotstack status response or its 'Response' property was null.");

        //    // Map StatusResponse to Model.RenderResponse
        //    return new Model.RenderResponse
        //    {
        //        Id = statusRes.Response.Id,
        //        Status = statusRes.Response.Status,
        //        Url = statusRes.Response.Url
        //    };
        //}

        public async Task<Model.RenderResponse> RenderAsync(string audioUrl, string? backgroundUrl, double lengthSec, string title)
        {
            // Build clips
            var clips = new List<object>
    {
        new { asset = new { type = "audio", src = audioUrl }, start = 0, length = lengthSec }
    };

            if (!string.IsNullOrWhiteSpace(backgroundUrl))
            {
                clips.Add(new { asset = new { type = "video", src = backgroundUrl }, start = 0, length = lengthSec });
            }

            clips.Add(new
            {
                asset = new { type = "title", text = title, style = "minimal", position = "bottom" },
                start = 0,
                length = Math.Min(5, lengthSec)
            });

            //var payload = new
            //{
            //    timeline = new { tracks = new[] { new { clips } } },
            //    output = new { format = "mp4", resolution = "hd" }
            //};
            var payload = new
            {
                timeline = new { background = "#000000", tracks = new[] { new { clips } } },
                output = new { format = "mp4", resolution = "hd" }
            };


            // --- Render request ---
            var res = await _http.PostAsJsonAsync("render", payload);

            if (!res.IsSuccessStatusCode)
            {
                var errorText = await res.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Shotstack API error: {res.StatusCode} - {errorText}");
            }

            var renderRes = await res.Content.ReadFromJsonAsync<StatusResponseWrapper>();
            if (renderRes?.Response == null)
            {
                var raw = await res.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Shotstack render response was null. Raw: {raw}");
            }

            // --- Poll status ---
            //var res1 = await _http.GetAsync($"render/{renderRes.Response.Id}");
            //if (!res1.IsSuccessStatusCode)
            //{
            //    var errorText = await res1.Content.ReadAsStringAsync();
            //    throw new InvalidOperationException($"Shotstack status API error: {res1.StatusCode} - {errorText}");
            //}

            //var statusRes = await res1.Content.ReadFromJsonAsync<StatusResponseWrapper>();
            //if (statusRes?.Response == null)
            //{
            //    var raw = await res1.Content.ReadAsStringAsync();
            //    throw new InvalidOperationException($"Shotstack status response was null. Raw: {raw}");
            //}

            //// --- Map to your model ---
            //return new Model.RenderResponse
            //{
            //    Id = statusRes.Response.Id,
            //    Status = statusRes.Response.Status,
            //    Url = statusRes.Response.Url
            //};
            StatusResponseWrapper? statusRes = null;
            do
            {
                await Task.Delay(2000); // wait 2 seconds
                var poll = await _http.GetAsync($"render/{renderRes.Response.Id}");
                poll.EnsureSuccessStatusCode();
                statusRes = await poll.Content.ReadFromJsonAsync<StatusResponseWrapper>();
            }
            while (statusRes?.Response?.Status != "done" && statusRes?.Response?.Status != "failed");

            return new Model.RenderResponse
            {
                Id = statusRes.Response.Id,
                Status = statusRes.Response.Status,
                Url = statusRes.Response.Url
            };

        }


        public async Task<Model.RenderStatus> GetStatusAsync(string id)
            {
                var res = await _http.GetAsync($"render/{id}");
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<Model.RenderStatus>() ?? new Model.RenderStatus();
            }
        }

        public class ShotstackOptions
        {
            public string BaseUrl { get; set; } = "https://api.shotstack.io/stage/";
            public string ApiKey { get; set; } = "";
        }
    public class ShotstackResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("message")] public string Message { get; set; }
        [JsonPropertyName("response")] public RenderResponse Response { get; set; }
    }

    public class ShotstackResponseData
    {
        [JsonPropertyName("message")] public string Message { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
    }
    public class StatusResponseWrapper
    {
        [JsonPropertyName("response")]
        public StatusResponse Response { get; set; }
    }

    public class StatusResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } // appears when status = "done"
    }


}
