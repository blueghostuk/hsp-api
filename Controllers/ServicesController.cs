using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using hsp_api.Model;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace hsp_api.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly Configuration _settings;
        private readonly ILogger _logger;

        public ServicesController(IOptions<Configuration> settings, ILogger<ServicesController> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        [HttpGet]
        [Route("{fromCRS}/{toCRS}/")]
        public async Task<IActionResult> Get(string fromCRS, string toCRS, DateTime? startDate = null, DateTime? endDate = null)
        {
            fromCRS = fromCRS.ToUpper();
            toCRS = toCRS.ToUpper();
            startDate = startDate ?? DateTime.UtcNow.AddHours(-2);
            endDate = endDate ?? DateTime.UtcNow;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_settings.Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.AcceptEncoding.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", _settings.Username, _settings.Password))));
                var response = await client.PostAsJsonAsync<ServiceMetricsRequest>("api/v1/serviceMetrics/", new ServiceMetricsRequest(
                    fromCRS,
                    toCRS,
                    startDate.Value,
                    endDate.Value
                ));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var serviceMetrics = JsonConvert.DeserializeObject<ServiceMetricsResponse>(content);
                    var services = serviceMetrics.ServiceDetails
                        .Select(s => GetServiceDetails(s.ServiceId))
                        .ToArray();
                    await Task.WhenAll(services);

                    return Ok(TrainMapper.MapTo(fromCRS, toCRS, services.Select(t => t.Result)));
                }
                else
                {
                    _logger.LogWarning("Response not successful: {0}", response.StatusCode);
                }
                return new StatusCodeResult((int)response.StatusCode);
            }
        }

        private async Task<ServiceDetailsResponse> GetServiceDetails(string rid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_settings.Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.AcceptEncoding.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", _settings.Username, _settings.Password))));
                var response = await client.PostAsJsonAsync<ServiceDetailsRequest>("api/v1/serviceDetails/", new ServiceDetailsRequest(rid));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var details = JsonConvert.DeserializeObject<ServiceDetailsResponse>(content);
                    if (details == null)
                    {
                        _logger.LogWarning("Could not parse service details: {0}", content);
                    }
                    return details;
                }
                else
                {
                    _logger.LogWarning("Could not find service details for: {0}. HTTP Code {1}",
                        rid,
                        response.StatusCode);
                }
            }

            return null;
        }
    }
}
