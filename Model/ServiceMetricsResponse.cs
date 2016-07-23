using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace hsp_api.Model
{
    public sealed class ServiceMetricsResponse
    {
        public IEnumerable<Service> Services { get; set; }

        public IEnumerable<ServiceAttributesMetrics> ServiceDetails
        {
            get
            {
                return Services.Select(s => s.ServiceDetails);
            }
        }
    }

    public sealed class Service
    {
        [JsonProperty(PropertyName = "serviceAttributesMetrics")]
        public ServiceAttributesMetrics ServiceDetails { get; set; }
    }

    public sealed class ServiceAttributesMetrics
    {
        [JsonProperty(PropertyName = "origin_location")]
        public string OriginLocation { get; set; }
        [JsonProperty(PropertyName = "destination_location")]
        public string DestinationLocation { get; set; }

        [JsonProperty(PropertyName = "gbtt_pta")]
        public string PublicArrival { get; set; }

        [JsonProperty(PropertyName = "gbtt_ptd")]
        public string PublicDeparture { get; set; }

        [JsonProperty(PropertyName = "toc_code")]
        public string TOCCode { get; set; }

        [JsonProperty(PropertyName = "rids")]
        public IEnumerable<string> ServiceIds { get; set; }

        public string ServiceId { get { return ServiceIds.First(); } }
    }

}