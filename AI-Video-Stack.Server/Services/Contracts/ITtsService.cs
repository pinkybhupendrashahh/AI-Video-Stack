namespace AI_Video_Stack.Server.Services.Contracts
{
    public interface ITtsService
    {
        Task<string> SynthesizeAsync(string text, string voice = "default");
      // Task<TtsResult> SynthesizeAsync(string text, string voice = "default");
      }
    public class TtsResult
    {
        public string PublicUrl { get; set; } = "";
    }

}

