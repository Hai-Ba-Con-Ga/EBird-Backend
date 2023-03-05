using EBird.Application.Exceptions;
using EBird.Application.Services.IServices;
using EBird.Application.Services.Algorithm;
using Newtonsoft.Json;
using AutoMapper.Configuration.Annotations;

namespace EBird.Application.Services
{
    public class MatchingService
    {
        public static readonly double MatchRatio = 80/100; //80%
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

        public static List<(Guid,Guid)> BinarySearch(List<RequestTuple> listRequest) {
            var left = 0.0;
            var right = EdgeDistance.MaxCapacity() + 0.5;
            var minEdge =  right + 0.5;
            var initalPair = PerfectMatching(listRequest).Solve();

            var init = PerfectMatching(listRequest);
            Console.WriteLine(init.Solve());
            foreach (var item in init.GetMate())
            {
                Console.WriteLine(item);
            }

            while (Math.Abs(right - left) > 0.001)
            {
                var mid = (left + right) / 2;
                if (CanMatchWithCapacity(mid, listRequest, initalPair))
                {
                    right = mid;
                    minEdge = Math.Min(minEdge, mid);
                }
                else left = mid;
            }
            var pairList = PerfectMatching(listRequest);
            pairList.Solve();
            
            foreach (var item in pairList.GetMate())
            {
                Console.WriteLine(item);
            }


            //var resultList = new List<(RequestTuple, RequestTuple)>();
            var resultList = new List<(Guid, Guid)>();

            for (int i = 0; i < listRequest.Count; i++)
            {
                var x = pairList.GetMate()[i];
                if (i < x)
                {
                    resultList.Add((listRequest[i].id, listRequest[x].id));
                }
            }
            return resultList;
        }

        public static Blossom PerfectMatching(List<RequestTuple> listRequest, double capacity = Int32.MaxValue)
        {
            return GraphInitailization(listRequest, capacity);

        }

        public static bool CanMatchWithCapacity(double capacity, List<RequestTuple> listRequest, int n)
        {
            var matching = PerfectMatching(listRequest, capacity);
            return (double)matching.Solve() / (double)n >= MatchRatio;
        }

        public static Blossom GraphInitailization(List<RequestTuple> listRequest, double capacity) {
            var graph = new Blossom(listRequest.Count);
            for (int i = 0; i < listRequest.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    //validate 2 request from same user. not yet.

                    var tmp = EdgeDistance.CapacityOfTwoRequest(listRequest[i], listRequest[j]);
                    
                    if (0 <= tmp && tmp <= capacity)
                    {
                        graph.AddEdge(i, j);
                    }
                }
            }
            return graph;
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
