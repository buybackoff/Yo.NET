using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Ractor;

namespace Yo.Services
{
    /// <summary>
    /// Create base actor for each related actors, set required settings
    /// </summary>
    public class YoBaseActor<TTask, TResult> : Actor<TTask, TResult> {

        /// <summary>
        /// Where Ractors store messages and other service info (namespace "R")
        /// </summary>
        public override string RedisConnectionString { get { return "localhost"; } }

        /// <summary>
        /// Where application data is stored
        /// </summary>
        public override string RedisDataConnectionString { get { return "localhost"; } }

        /// <summary>
        /// Redis namespace for application data
        /// </summary>
        public override string RedisDataNamespace { get { return "Yo.NET"; } }
    }


    public class YoCounter : YoBaseActor<string, long> {
        public override async Task<long> Computation(string username) {
            var userCount = await Redis.GetAsync<long>("yo:counter:" + (username ?? "_total_") );
            return userCount;
        }
    }

    public class YoIncrementer : YoBaseActor<Tuple<string, string>, string> {
        public override async Task<string> Computation(Tuple<string, string> usernameMessage) {
            // unpack tuple, Tuples are ugly and inconvenient in C#
            var userName = usernameMessage.Item1;
            var message = usernameMessage.Item2;

            await Redis.IncrAsync("yo:counter:" + (userName ?? "_total_"));

            if (!string.IsNullOrEmpty(message) && message != "+2") {
                // get total count by calling counter actor from this actor
                var totalCount = await new YoCounter().PostAndGetResultAsync(null);

                var messageLine = totalCount + ". "
                                  + userName + ": " + message;
                // save message line
                await Redis.RPushAsync<string>("yo:messages", messageLine);

                // notify all clients about a new message from an actor
                var yoHub = GlobalHost.ConnectionManager.GetHubContext<YoHub>();
                yoHub.Clients.All.AddMessage(messageLine);
                
                
            }
            return userName;
        }
    }

    public class YoMessageHistoryGetter : YoBaseActor<int, string[]> {
        public override async Task<string[]> Computation(int historyLength) {
            var hist = await Redis.LRangeAsync<string>("yo:messages", 0, historyLength);
            return hist;
        }
    }

}
