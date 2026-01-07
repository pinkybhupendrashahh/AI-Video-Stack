namespace AI_Video_Stack.Server.Services.Contracts
{
    public interface IShotstackService
    {
        //Task<RenderResponse> RenderAsync(string audioUrl, string backgroundUrl, double lengthSec, string title);
        //Task<RenderStatus> GetStatusAsync(string id);
        Task<Model.RenderStatus> GetStatusAsync(string id);
        Task<Model.RenderResponse> RenderAsync(string audioUrl, string? backgroundUrl, double lengthSec, string title);
    }
   // public record RenderResponse(string id, string status); public record RenderStatus(string id, string status, string? url);
}
