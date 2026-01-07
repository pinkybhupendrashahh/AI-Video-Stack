using AI_Video_Stack.Server.Services;
using System.Text.Json.Serialization;

namespace AI_Video_Stack.Server.Model
{
    public class RenderResponse { public string Id { get; set; } = ""; public string Status { get; set; } = "";
        public string Url { get; internal set; }
    }
    
    public class RenderStatus { public string Id { get; set; } = ""; public string Status { get; set; } = ""; public string Url { get; set; } = ""; }
    public class ResponseWrapper { [JsonPropertyName("response")] public RenderResponse Response { get; set; } }
}
