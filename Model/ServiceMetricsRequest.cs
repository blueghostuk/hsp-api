using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace hsp_api.Model
{
    public sealed class ServiceMetricsRequest
    {
        public string from_loc { get; }

        public string to_loc { get; }

        public string from_time { get; }

        public string to_time { get; }

        public string from_date { get; }

        public string to_date { get; }

        public string days { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> toc_filter { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<uint> tolerance { get; }

        public ServiceMetricsRequest(string fromCRS, string toCRS, DateTime dateFrom, DateTime dateTo, IEnumerable<string> tocFilter = null, IEnumerable<uint> tolerance = null)
        {
            from_loc = fromCRS;
            to_loc = toCRS;

            if (dateFrom.Date != dateTo.Date)
            {
                throw new ArgumentException("start and end dates must be the same day");
            }

            from_time = dateFrom.ToString("HHmm");
            from_date = dateFrom.ToString("yyyy-MM-dd");
            to_time = dateTo.ToString("HHmm");
            to_date = dateTo.ToString("yyyy-MM-dd");

            days = GetDays(dateFrom.DayOfWeek);

            toc_filter = tocFilter;
            this.tolerance = tolerance;
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