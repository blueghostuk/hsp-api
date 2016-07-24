using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace hsp_api.Model
{
    internal sealed class ServiceDetailsResponse
    {
        [JsonProperty(PropertyName = "serviceAttributesDetails")]
        public ServiceDetails ServiceDetails { get; set; }
    }

    internal sealed class ServiceDetails
    {
        [JsonProperty(PropertyName = "date_of_service")]
        public DateTime DateOfService { get; set; }

        [JsonProperty(PropertyName = "toc_code")]
        public string TOC { get; set; }

        [JsonProperty(PropertyName = "rid")]
        public string ServiceId { get; set; }

        [JsonProperty(PropertyName = "locations")]
        public IEnumerable<CallingPoint> CallingPoints { get; set; }
    }

    internal sealed class CallingPoint
    {
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "gbtt_ptd")]
        public string PublicDeparture { get; set; }

        [JsonProperty(PropertyName = "actual_td")]
        public string ActualDeparture { get; set; }

        [JsonProperty(PropertyName = "gbtt_pta")]
        public string PublicArrival { get; set; }

        [JsonProperty(PropertyName = "actual_ta")]
        public string ActualArrival { get; set; }

        [JsonProperty(PropertyName = "late_canc_reason")]
        public string LateCancelReason { get; set; }
    }
}