using System;
using System.Collections.Generic;

namespace hsp_api.Model
{
    public class Train
    {
        public string Operator { get; set; }
        public Stop From { get; set; }
        public Stop To { get; set; }

        public IEnumerable<Stop> AllStops { get; set; }
    }

    public sealed class Stop
    {
        public TimeSpan? ExpectedDeparture { get; set; }
        public TimeSpan? ActualDeparture { get; set; }
        public double? DepartureDelay
        {
            get
            {
                return ExpectedDeparture.HasValue && ActualDeparture.HasValue ?
                    (ActualDeparture.Value - ExpectedDeparture.Value).TotalMinutes : default(double?);
            }
        }

        public TimeSpan? ExpectedArrival { get; set; }
        public TimeSpan? ActualArrival { get; set; }
        public double? ArrivalDelay
        {
            get
            {
                return ExpectedArrival.HasValue && ActualArrival.HasValue ?
                    (ActualArrival.Value - ExpectedArrival.Value).TotalMinutes : default(double?);
            }
        }
        public string Location { get; set; }
        public string LateCancelReason { get; set; }
    }
}