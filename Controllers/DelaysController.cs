using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using hsp_api.Model;
using System.Net;

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

        // GET api/values
        [HttpGet]
        public async Task<string> Get(string fromCRS, string toCRS, DateTime? startDate = null, DateTime? endDate = null)
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
                    return await response.Content.ReadAsStringAsync();
                    //var responseJson = await response.Content.ReadAsAsync<string[]>();
                    //do something with the response here. Typically use JSON.net to deserialise it and work with it
                }
            }

            return string.Empty;
        }
    }
}
