using System;
using System.Threading.Tasks;
using ServiceStack;

namespace Services {

    [Route("/counter")]
    public class Counter { }
    public class CounterResponse {
        public long TotalCounter { get; set; }
        public long UserCounter { get; set; }
    }

    /// <summary>
    /// Every call to count service increments the stored counter
    /// </summary>
    public class CounterService : HttpServiceBase {
        public async Task<CounterResponse> Any(Counter request) {
            // TODO here showcase Fredis actors - total counter doesn't depend on state
            // we could also get user counter via actor by providing sessionId
            
            // TODO INCR not yet implemented in Fredis
            var count = await Redis.GetAsync<long>("yo:counter", true) + 1;
            await Redis.SetAsync("yo:counter", count);

            // TODO logically there are two services, one ServiceBase doesn;t depend on 
            // TODO Request/Session while the second one does. Think about distinction and actors
            // ServiceStack and SignalR are kind of controllers/distpatchers in a big MVVM:
            // http://en.wikipedia.org/wiki/Model_View_ViewModel
            // Model is all data in dbs, redis, s3 - and Actors that do data processing
            // VM (or controller/dispatcher) is SS or SR - they route request to models, they check auth and do other infrastructure logic like cache etc
            // View - Clients

            var userCount = await Redis.GetAsync<long>("yo:counter:" + SessionId, true) + 1;
            await Redis.SetAsync("yo:counter:" + SessionId, userCount, TimeSpan.FromDays(7));

            return new CounterResponse { TotalCounter = count, UserCounter = userCount };
        }
    }
}