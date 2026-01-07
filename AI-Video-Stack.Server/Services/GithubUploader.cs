namespace AI_Video_Stack.Server.Services
{// Services/GithubUploader.cs
    using Microsoft.Extensions.Options;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;

    public class GithubUploader
    {
        private readonly HttpClient _http;
        private readonly GithubOptions _opt;

        public GithubUploader(IHttpClientFactory factory, IOptions<GithubOptions> opt , IConfiguration config)
        {
            _http = factory.CreateClient("Github");
            _opt = opt.Value;
            _http.BaseAddress = new Uri("https://api.github.com/");
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("AI-Video-Stack");
         //   _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _opt.Token);
            var token = config["Github:Token"]; // comes from user-secrets or env
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
        }

        public async Task<string> UploadAsync(string localFilePath, string targetFileName)
        {
            var contentB64 = Convert.ToBase64String(await File.ReadAllBytesAsync(localFilePath));
            var payload = new
            {
                message = $"Add {targetFileName}",
                committer = new { name = _opt.CommitterName, email = _opt.CommitterEmail },
                content = contentB64,
                branch = _opt.Branch
            };

            var url = $"repos/{_opt.Owner}/{_opt.Repo}/contents/{_opt.AssetsFolder}/{targetFileName}";
            var res = await _http.PutAsJsonAsync(url, payload);
            res.EnsureSuccessStatusCode();

            // Public GitHub Pages URL
            return $"https://{_opt.Owner}.github.io/{_opt.Repo}/{_opt.AssetsFolder}/{targetFileName}";
        }
    }

    public class GithubOptions
    {
        public string Token { get; set; } = "";            // PAT with 'repo' scope
        public string Owner { get; set; } = "";            // github username or org
        public string Repo { get; set; } = "video-assets"; // public repo serving Pages
        public string Branch { get; set; } = "main";
        public string AssetsFolder { get; set; } = "assets";
        public string CommitterName { get; set; } = "AI Video Bot";
        public string CommitterEmail { get; set; } = "bot@example.com";
    }

}
