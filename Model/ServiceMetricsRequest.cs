using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace hsp_api.Model
{
    internal sealed class ServiceMetricsRequest
    {
        [JsonProperty(PropertyName="from_loc")]
        public string FromLocation { get; }

        [JsonProperty(PropertyName="to_loc")]
        public string ToLocation { get; }

        [JsonProperty(PropertyName="from_time")]
        public string FromTime { get; }

        [JsonProperty(PropertyName="to_time")]
        public string ToTime { get; }

        [JsonProperty(PropertyName="from_date")]
        public string FromDate { get; }

        [JsonProperty(PropertyName="to_date")]
        public string ToDate { get; }

        [JsonProperty(PropertyName="days")]
        public string Days { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName="toc_filter")]
        public IEnumerable<string> TOCFilter { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName="tolerance")]
        public IEnumerable<uint> Tolerance { get; }

        public ServiceMetricsRequest(string fromCRS, string toCRS, DateTime dateFrom, DateTime dateTo, IEnumerable<string> tocFilter = null, IEnumerable<uint> tolerance = null)
        {
            FromLocation = fromCRS;
            ToLocation = toCRS;

            if (dateFrom.Date != dateTo.Date)
            {
                throw new ArgumentException("start and end dates must be the same day");
            }

            FromTime = dateFrom.ToString("HHmm");
            FromDate = dateFrom.ToString("yyyy-MM-dd");
            ToTime = dateTo.ToString("HHmm");
            ToDate = dateTo.ToString("yyyy-MM-dd");

            Days = GetDays(dateFrom.DayOfWeek);

            TOCFilter = tocFilter;
            this.Tolerance = tolerance;
        }

        private static string GetDays(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return "WEEDKDAY";
                case DayOfWeek.Saturday:
                    return "SATURDAY";
                case DayOfWeek.Sunday:
                    return "SUNDAY";
                default:
                    throw new ArgumentException("invalid day of week");
            }
        }
    }
}