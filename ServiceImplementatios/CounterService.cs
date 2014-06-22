using System;
using System.Threading.Tasks;
using ServiceImplementations.Common;
using ServiceStack;

namespace ServiceImplementations {

    [Route("/counter")]
    public class Counter { }
    public class CounterResponse {
        public long TotalCounter { get; set; }
        public long UserCounter { get; set; }
    }

    /// <summary>
    /// Every call to count service increments the stored counter
    /// </summary>
    public class CounterService : ServiceBase {
        public async Task<CounterResponse> Any(Counter request) {
            // TODO here showcase Fredis actors - total counter doesn't depend on state
            // we could also get user counter via actor by providing sessionId
            
            // TODO INCR not yet implemented in Fredis
            var count = await Redis.GetAsync<long>("yo:counter", true) + 1;
            await Redis.SetAsync("yo:counter", count);

            var userCount = await Redis.GetAsync<long>("yo:counter:" + SessionId, true) + 1;
            await Redis.SetAsync("yo:counter:" + SessionId, userCount, TimeSpan.FromDays(7));

            return new CounterResponse { TotalCounter = count, UserCounter = userCount };
        }
    }
}