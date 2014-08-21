using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Ractor;
using Yo.Contracts;
using AuthorizeAPI = System.Web.Http.AuthorizeAttribute;

namespace Yo.Services {


    [RoutePrefix("yo")]
    public class YoController : ControllerBase {
        readonly YoIncrementer _incrementerActor = new YoIncrementer();
        readonly YoCounter _counterActor = new YoCounter();
        readonly YoMessageHistoryGetter _historyActor = new YoMessageHistoryGetter();

        IHubContext _yoHub = GlobalHost.ConnectionManager.GetHubContext<YoHub>();


        [Route("")]
        [HttpPost]
        [AuthorizeAPI]
        public async Task<YoResponse> PostYo(YoRequest request) {

            // gets user names, increments Yo count for each users and returns same usernames
            var parallelIncrementer = _incrementerActor.ParallelWith(_incrementerActor);

            // gets user names and returns Yo count for each user
            var paralellCounter = _counterActor.ParallelWith(_counterActor);

            // will accept a tuple of tuples: ((UserName, Message),(UserName, Message))
            var yoActor = parallelIncrementer.ContinueWith(paralellCounter);

            // both username and message are nulls
            var messageForTotalCount = Tuple.Create<string, string>(null, null);

            // message from request and username from builtin identity
            var messageForUserCount = Tuple.Create(User.Identity.Name, request.Message);

            // combined message for total and user: ((null, null),(UserName, Message))
            var yoMessage = Tuple.Create(messageForTotalCount, messageForUserCount);

            // here incrementers will run in paralell, then when both results are ready
            // then counters will run in parallel
            var yoResult = await yoActor.PostAndGetResultAsync(yoMessage);

            
            if (request.Message == "+2") {
                // same signature, but increment and count run in parallel for each user
                // compose in one place
                var yoActor2 = (_incrementerActor.ContinueWith(_counterActor))
                    .ParallelWith(_incrementerActor.ContinueWith(_counterActor));
                var yoResult2 = await yoActor2.PostAndGetResultAsync(yoMessage);
                // item2 is for user
                // if no paralell request from the same user, this must be true
                Trace.Assert(yoResult.Item2 + 1 == yoResult2.Item2);
            }

            var response = new YoResponse {
                AllYos = yoResult.Item1,
                UserYos = yoResult.Item2
            };

            // notify clients from controller with new yo values
            _yoHub.Clients.All.SetAllYos(yoResult.Item1);
            // TODO: name inconsistency, should rename UserYos to MyYos in YoResponse above
            _yoHub.Clients.User(UserName).SetMyYos(yoResult.Item2);

            return response;

        }


        //[Route("")]
        //[HttpGet]
        //public async Task<YoResponse> GetYo() {
        //    //// call service from another service
        //    //var counter = await ResolveService<CounterService>().Any(null);

        //    //// get signalR HubContext
        //    //var yoHub = GetHubContext<YoHub>();

        //    //string checkedName;
        //    //if (string.IsNullOrEmpty(request.Name)) {
        //    //    checkedName = "Guest";
        //    //} else {
        //    //    checkedName = await Redis.HGetAsync<string>("yo:twitter", request.Name);
        //    //    if (checkedName == null) {
        //    //        var httpRequest = (HttpWebRequest)WebRequest.Create("https://twitter.com/" + request.Name);
        //    //        httpRequest.Method = WebRequestMethods.Http.Head;
        //    //        var pageExists = false;
        //    //        try {
        //    //            var response = (HttpWebResponse)httpRequest.GetResponse();
        //    //            pageExists = response.StatusCode == HttpStatusCode.OK;
        //    //            // ReSharper disable once EmptyStatement
        //    //        } catch { ;}

        //    //        if (pageExists) {
        //    //            await Redis.HSetAsync<string>("yo:twitter", request.Name, "@");
        //    //            checkedName = "<a href=\"https://twitter.com/" + request.Name + "\">@" + request.Name + "</a>";

        //    //        } else {
        //    //            await Redis.HSetAsync<string>("yo:twitter", request.Name, "");
        //    //            checkedName = "" + request.Name;
        //    //        }
        //    //    } else { checkedName = checkedName + request.Name; }
        //    //}
        //    //request.Name = checkedName;

        //    //var messageLine = counter.TotalCounter + ". "
        //    //                  + checkedName + ": " + request.Message;

        //    //// save
        //    //if (!string.IsNullOrEmpty(request.Message)) {
        //    //    await Redis.RPushAsync<string>("yo:messages", messageLine);
        //    //}
        //    //if (!string.IsNullOrEmpty(request.Message))
        //    //    yoHub.Clients.All.AddMessage(messageLine);
        //    //yoHub.Clients.All.SetAllYos(counter.TotalCounter);
        //    //yoHub.Clients.User(HubUserId).SetMyYos(counter.UserCounter);

        //    //var resp = new YoResponse {
        //    //    AllYos = counter.TotalCounter,
        //    //    MyYos = counter.UserCounter
        //    //};

        //    //if (request.WithHistory) {
        //    //    var hist = await Redis.LRangeAsync<string>("yo:messages", 0, 100);
        //    //    resp.History = hist;
        //    //}

        //    //return resp;
        //    var resp = new YoResponse {
        //        AllYos = 42,
        //        UserYos = 42,
        //        //History = new[] {"Dummy history"}
        //    };
        //    return resp; //await Task.FromResult(resp);
        //}


        [HttpGet]
        [AuthorizeAPI]
        [Route("")]
        public async Task<YoResponse> GetCounter() {
            var result = await _counterActor
                .ParallelWith(_counterActor)
                .PostAndGetResultAsync(Tuple.Create<string, string>(null, User.Identity.Name));
            return new YoResponse {
                AllYos = result.Item1,
                UserYos = result.Item2
            };
        }


        [HttpGet]
        [AuthorizeAPI]
        [Route("history")]
        public async Task<YoHistoryResponse> GetHistory() {
            var result = await _historyActor.PostAndGetResultAsync(100);
            return new YoHistoryResponse {
                History = result
            };
        }

    }
}