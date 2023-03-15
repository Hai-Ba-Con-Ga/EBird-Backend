using EBird.Application.Exceptions;
using EBird.Application.Services.IServices;
using EBird.Application.Services.Algorithm;
using Newtonsoft.Json;
using AutoMapper.Configuration.Annotations;
using EBird.Application.Validation;
using EBird.Application.Interfaces.IValidation;
using Org.BouncyCastle.Asn1;

namespace EBird.Application.Services
{
    public class MatchingService : IMatchingService
    {
        public IRequestValidation _requestValidation;
        public static readonly double MatchRatio = 0.8; //80%
        public static List<RequestTuple> QuickMatch(List<RequestTuple> listRequest, RequestTuple finder) {
            //var min = double.MaxValue;
            List<(RequestTuple, double)> result = new List<(RequestTuple, double)>();
            foreach(var item in listRequest) {
                var capacity = EdgeDistance.CapacityOfTwoRequest(finder, item);
                //Console.WriteLine(EdgeDistance.LocationDistance(10.8411455160116, 106.80985584436259, 10.857847264830015, 106.78786938795808));
                //Console.WriteLine(capacity + " + id: " + item.id + "\nday : " + EdgeDistance.DateDistance(finder.date,item.date));
                if (capacity < 0) continue;
                result.Add((item, capacity));
            }
            if (result.Count == 0) {
                throw new NotFoundException("Can not found any request match with this request.");
            }
            return result.OrderBy(x => x.Item2).Select(x => x.Item1).Take(5).ToList();

        }

        public async Task<List<Tuple<Guid,Guid>>> BinarySearch(List<RequestTuple> listRequest) {
            var left = 0.0;
            var right = EdgeDistance.MaxCapacity() + 0.5;
            var minEdge =  right + 0.5;
            var initalPair = (await PerfectMatching(listRequest)).Solve();
            //Console.WriteLine("initalPair: " + initalPair);

            while (Math.Abs(right - left) > 0.001)
            {
                var mid = (left + right) / 2;
                if (await CanMatchWithCapacity(mid, listRequest, initalPair))
                {
                    right = mid;
                    minEdge = Math.Min(minEdge, mid);
                }
                else left = mid;
            }
            var pairList = await PerfectMatching(listRequest, minEdge);
            pairList.Solve();
            //Console.WriteLine(pairList.Solve());
            //Console.WriteLine($"minEdge {minEdge}\ninital {initalPair}");

            //var resultList = new List<(RequestTuple, RequestTuple)>();
            var resultList = new List<Tuple<Guid, Guid>>();

            foreach(var (value, i) in pairList.GetMate().Select((value, i) => (value, i)))
            {
                if (i < value)
                    resultList.Add(Tuple.Create(listRequest[i].id, listRequest[value].id));
            }
            return resultList;
        }

        public async Task<Blossom> PerfectMatching(List<RequestTuple> listRequest, double capacity = Int32.MaxValue)
        {
            return await GraphInitailization(listRequest, capacity);

        }

        public async Task<bool> CanMatchWithCapacity(double capacity, List<RequestTuple> listRequest, int n)
        {
            var matching = await PerfectMatching(listRequest, capacity);
            return (double)matching.Solve() / (double)n >= MatchRatio;
        }

        public async Task<Blossom> GraphInitailization(List<RequestTuple> listRequest, double capacity) {
            var graph = new Blossom(listRequest.Count);
            //Console.WriteLine("capacity" + capacity);
            for (int i = 0; i < listRequest.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    //validate 2 request from same user. not yet.

                    var tmp = EdgeDistance.CapacityOfTwoRequest(listRequest[i], listRequest[j]);
                    var same = listRequest[i].hostId == listRequest[j].hostId;
                    if (!same && 0 <= tmp && tmp <= capacity)
                    {
                        //if (capacity < 2.09482027053833)
                        //Console.WriteLine($"{i} {j} {tmp}");
                        graph.AddEdge(i, j);
                    }
                }
            }
            return graph;
        }

		public dynamic LoadJson(string f)
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
