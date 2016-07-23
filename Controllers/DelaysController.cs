using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using hsp_api.Model;
using Newtonsoft.Json;

namespace hsp_api.Controllers
{
    [Route("api/[controller]")]
    public class DelaysController : Controller
    {
        private readonly Configuration _settings;

        public DelaysController(IOptions<Configuration> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet]
        public async Task<ServiceMetricsResponse> Get(string fromCRS, string toCRS, DateTime? startDate = null, DateTime? endDate = null)
        {
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
                    return JsonConvert.DeserializeObject<ServiceMetricsResponse>(content);
                }
            }

            return null;
        }

        [HttpGet]
        [Route("service")]
        public async Task<ServiceDetailsResponse> GetServiceDetails(string rid)
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
                    return JsonConvert.DeserializeObject<ServiceDetailsResponse>(content);
                }
            }

            return null;
        }
    }
}
