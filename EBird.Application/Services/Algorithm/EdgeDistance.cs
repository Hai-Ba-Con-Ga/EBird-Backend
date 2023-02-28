namespace EBird.Application.Services.Algorithm
{
    public class EdgeDistance
    {

        // limit between two requests
        public static readonly double LocationLimit = 1000;
        public static readonly int EloLimit = 400;
        public static readonly int DateLimit = 7;

        //  20km ~ 100elo
        public static readonly double RatioLocationElo = 0;

        // 20km ~ 7days
        public static readonly double RatioLocationDate = 0;
        
        public static double DegToRad(double deg)
        {
            return deg * Math.PI / 180;
        }
        public static double LocationDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371;  // Earth radius in kilometers
            double dLat = DegToRad(lat2 - lat1);
            double dLon = DegToRad(lon2 - lon1);
            lat1 = DegToRad(lat1);
            lat2 = DegToRad(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            return distance;
        }

        public static int EloDistance(int eloRequest1, int eloRequest2)
        {
            return Math.Abs(eloRequest1 - eloRequest2);
        }

        public static int DateDistance(DateTime dateRequest1, DateTime dateRequest2)
        {
            return Math.Abs((dateRequest1 - dateRequest2).Days);
        }

        public static double MaxCapacity() {
            return Math.Sqrt(Math.Pow(LocationLimit, 2) + 
                             Math.Pow(EloLimit, 2) * RatioLocationElo + 
                             Math.Pow(DateLimit, 2) * RatioLocationDate);
        }

        public static double CapacityOfTwoRequest(RequestTuple request1, RequestTuple request2)
        {
            //validation
            //if (!IsLocationSatisfied(request1, request2)) return -1;
            //if (!IsEloSatisfied(request1, request2)) return -1;
            //if (!IsDateSatisfied(request1, request2)) return -1;
            // IsCapacitySatisfied...

            // calculate the distance between 3 weights of two requests
            double locationDistance = LocationDistance(request1.location.latitude, request1.location.longitude, request2.location.latitude, request2.location.longitude);
            int eloDistance = EloDistance(request1.elo, request2.elo);
            int dateDistance = DateDistance(request1.date, request2.date);

            // calculate the capacity of two requests
            double capacity = Math.Sqrt(Math.Pow(locationDistance, 2) + 
                                        Math.Pow(eloDistance, 2) * RatioLocationElo + 
                                        Math.Pow(dateDistance, 2) * RatioLocationDate);
            
            //validation capacity.

            return capacity;
        }

        public static bool IsLocationSatisfied(RequestTuple request1, RequestTuple request2)
        {
            double locationDistance = LocationDistance(request1.location.latitude, request1.location.longitude, request2.location.latitude, request2.location.longitude);
            return locationDistance <= LocationLimit;
        }
        public static bool IsEloSatisfied(RequestTuple request1, RequestTuple request2)
        {
            int eloDistance = EloDistance(request1.elo, request2.elo);
            return eloDistance <= EloLimit;
        }
        public static bool IsDateSatisfied(RequestTuple request1, RequestTuple request2)
        {
            int dateDistance = DateDistance(request1.date, request2.date);
            return dateDistance <= DateLimit;
        }
    }

}
