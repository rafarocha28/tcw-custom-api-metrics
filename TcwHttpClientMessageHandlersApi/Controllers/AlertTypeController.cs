using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using TcwHttpClientMessageHandlersApi.Options;

namespace TcwHttpClientMessageHandlersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertTypeController : ControllerBase, IController
    {        
        private readonly TcwOptions _options;

        private readonly IHttpClientFactory _httpClientFactory;

        public AlertTypeController(IHttpClientFactory httpClientFactory, IOptionsMonitor<TcwOptions> options)
        {
            _options = options.CurrentValue;
            _httpClientFactory = httpClientFactory;            
        }

        [HttpGet]        
        public async Task<IActionResult> Get(CancellationToken token)
        {            
            var response = await GetHttpClient()
                .GetAsync("/v1/alert-type?page=1&pageSize=10", token)
                .ConfigureAwait(false);
            if (response == null)
            {
                return NotFound();
            }
            if (response.IsSuccessStatusCode)
            {                
                return Ok();
            }

            return BadRequest();
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("time-handler");
            httpClient.Timeout = TimeSpan.FromSeconds(180);
            httpClient.BaseAddress = new Uri(_options.URL);
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer " + _options.Token);
            return httpClient;
        }
    }
}