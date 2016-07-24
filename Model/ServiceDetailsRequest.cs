using Newtonsoft.Json;

namespace hsp_api.Model
{
    internal sealed class ServiceDetailsRequest
    {
        [JsonProperty(PropertyName = "rid")]
        public string ServiceId { get; }

        public ServiceDetailsRequest(string serviceId)
        {
            ServiceId = serviceId;
        }
    }
}