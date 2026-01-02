namespace AI_Video_Stack.Server.Services.Contracts
{
    public interface IOllamaService
    {
        Task<string> GenerateAsync(string prompt);  
    }
}
