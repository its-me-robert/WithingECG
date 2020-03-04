using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;
using WithingsECG.API.Services.Model;
using WithingsECG.API.Services.Model.HeartGetResponse;
using WithingsECG.API.Services.Model.HeartListResponse;

namespace WithingsECG.API.Controllers
{
    public class WithingsService
    {
        private readonly string _baseUrlAuth = null;
        private readonly string _baseUrlHeart = null;
        private readonly string _clientId = null;
        private readonly string _clientSecret = null;
        private readonly string _redirectUrl = null;
        private static readonly string STATE = "witingsecg";
        private static readonly string SCOPES = "user.metrics,user.activity";

        public WithingsService(IConfiguration configuration)
        {
            _clientSecret = configuration["Withings:ClientSecret"];
            if (string.IsNullOrEmpty(_clientSecret))
            {
                throw new System.Exception("Withings:ClientSecret not found (in secrets.json)");
            }
            _baseUrlHeart = configuration["Withings:BaseUrlHeart"];
            _baseUrlAuth = configuration["Withings:BaseUrlAuth"];
            _clientId = configuration["Withings:ClientId"];
            _redirectUrl = configuration["Withing:redirectUrl"];
        }

        public string AuthUrl => $@"{_baseUrlAuth}oauth2_user/authorize2?response_type=code&client_id={_clientId}&state={STATE}&redirect_uri={_redirectUrl}&scope={SCOPES}";

        public TokenResponse GetToken(string code)
        {
            using WebClient wClient = new WebClient();
            wClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var data = $@"client_id={_clientId}&client_secret={_clientSecret}&code={code}&redirect_uri={_redirectUrl}";
            string HtmlResult = wClient.UploadString($"{_baseUrlAuth}oauth2/token?grant_type=authorization_code", data);
            return JsonSerializer.Deserialize<TokenResponse>(HtmlResult, null);

        }

        public HeartListResponse ListECGs(string accessToken)
        {
            using WebClient wClient = new WebClient();
            SetAuthorization(accessToken, wClient);
            var result = wClient.DownloadString($"{_baseUrlHeart}v2/heart?action=list");
            return JsonSerializer.Deserialize<HeartListResponse>(result);
        }

        public HeartGetResponse GetECG(string accessToken, int signalid)
        {
            using WebClient wClient = new WebClient();
            SetAuthorization(accessToken, wClient);
            var result = wClient.DownloadString($"{_baseUrlHeart}v2/heart?action=get&signalid={signalid}");
            return JsonSerializer.Deserialize<HeartGetResponse>(result);

        }
        private void SetAuthorization(string accessToken, WebClient wClient)
        {
            wClient.Headers[HttpRequestHeader.Authorization] = $"Bearer {accessToken}";
        }


    }
}
