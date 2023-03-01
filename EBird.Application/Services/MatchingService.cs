using EBird.Application.Exceptions;
using EBird.Application.Services.IServices;
using EBird.Application.Services.Algorithm;
using Newtonsoft.Json;


namespace EBird.Application.Services
{
    public class MatchingService
    {
        public static List<RequestTuple> QuickMatch(List<RequestTuple> listRequest, RequestTuple finder) {
            //var min = double.MaxValue;
            List<(RequestTuple, double)> result = new List<(RequestTuple, double)>();
            foreach(var item in listRequest) {
                var capacity = EdgeDistance.CapacityOfTwoRequest(finder, item);
                //Console.WriteLine(capacity);
                if (capacity < 0) continue;
                // System.Console.WriteLine("capacity = " + capacity);
                //if (capacity < min) {
                //    min = capacity;
                //    result = item;
                //}
                result.Add((item, capacity));
            }
            if (result.Count == 0) {
                throw new NotFoundException("Can not found any request match with this request.");
            }
            return result.OrderBy(x => x.Item2).Select(x => x.Item1).Take(5).ToList();

        }

        public static void BinarySearch(List<RequestTuple> listRequest) {
            var left = 0.0;
            var right = EdgeDistance.MaxCapacity() + 0.5;
            var result =  right + 0.5;

            while (Math.Abs(right - left) > 0.001) {
                //var mid = (left + right) / 2;
                //if (CanMatchWithCapacity(mid)) {
                //    right = mid;
                //    result = Math.Min(result, mid);
                //}
                //else left = mid;
            }
        }

        public static void GraphInitailization() {
            
        }

        public void testQuickMatch() {

        }

		public static dynamic LoadJson(string f)
		{
			using (StreamReader r = new StreamReader("./MockData/" + f))
			{
				string json = r.ReadToEnd();
				dynamic array = JsonConvert.DeserializeObject(json);
				return array;
			}
		}
        
    }

}
