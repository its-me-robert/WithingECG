using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WithingsECG.API.Controllers;
using WithingsECG.API.Services.Model.HeartListResponse;

namespace Withings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithingsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly WithingsService _withingService;

        public WithingsController(IConfiguration config)
        {
            _config = config;
            _withingService = new WithingsService(_config);
        }

        [HttpGet]
        public RedirectResult GetAuthUrl()
        {
            return Redirect(_withingService.AuthUrl);
        }

        [HttpGet]
        [Route("getecg")]
        public HeartListResponse GetECGS(string code)
        {
            var token = _withingService.GetToken(code);
            var heartGetResponse = _withingService.ListECGs(token.access_token);
            heartGetResponse.body.series.ToList().ForEach(e => e.ecg.HeartGetResponse = _withingService.GetECG(token.access_token, e.ecg.signalid));

            return heartGetResponse;
        }
    }
}