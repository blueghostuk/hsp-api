using System;
using System.Collections.Generic;
using System.Linq;

namespace hsp_api.Model
{
    internal static class TrainMapper
    {
        internal static IEnumerable<Train> MapTo(string fromCRS, string toCRS, IEnumerable<ServiceDetailsResponse> serviceDetails)
        {
            return serviceDetails.Select(sd => MapTo(fromCRS, toCRS, sd));
        }

        internal static Train MapTo(string fromCRS, string toCRS, ServiceDetailsResponse serviceDetails)
        {
            var allStops = serviceDetails.ServiceDetails.CallingPoints.Select(cp => MapTo(cp));

            Stop fromStop = null,
                toStop = null;

            foreach (var stop in allStops)
            {
                if (fromStop == null && stop.Location == fromCRS)
                {
                    fromStop = stop;
                }
                else if (fromStop != null && toStop == null && stop.Location == toCRS)
                {
                    toStop = stop;
                    break;
                }
            }

            return new Train
            {
                Operator = serviceDetails.ServiceDetails.TOC,
                From = fromStop,
                To = toStop,
                AllStops = allStops
            };
        }

        internal static Stop MapTo(CallingPoint callingPoint)
        {
            return new Stop
            {
                Location = callingPoint.Location,
                LateCancelReason = callingPoint.LateCancelReason,
                ExpectedDeparture = GetTimeSpan(callingPoint.PublicDeparture),
                ActualDeparture = GetTimeSpan(callingPoint.ActualDeparture),
                ExpectedArrival = GetTimeSpan(callingPoint.PublicArrival),
                ActualArrival = GetTimeSpan(callingPoint.ActualArrival)
            };
        }

        private static TimeSpan? GetTimeSpan(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return TimeSpan.ParseExact(input, "hhmm", null);
        }
    }
}