using System;
using System.Threading.Tasks;
using System.Web.Http;
using Ractor;
using Yo.Actors;
using Yo.Contracts;

namespace Yo.Services {


    [RoutePrefix("_/yo")]
    public class YoController : ControllerBase {

        [Route("")]
        [HttpGet]
        public YoResponse GetYo() {
            //// call service from another service
            //var counter = await ResolveService<CounterService>().Any(null);

            //// get signalR HubContext
            //var yoHub = GetHubContext<YoHub>();

            //string checkedName;
            //if (string.IsNullOrEmpty(request.Name)) {
            //    checkedName = "Guest";
            //} else {
            //    checkedName = await Redis.HGetAsync<string>("yo:twitter", request.Name);
            //    if (checkedName == null) {
            //        var httpRequest = (HttpWebRequest)WebRequest.Create("https://twitter.com/" + request.Name);
            //        httpRequest.Method = WebRequestMethods.Http.Head;
            //        var pageExists = false;
            //        try {
            //            var response = (HttpWebResponse)httpRequest.GetResponse();
            //            pageExists = response.StatusCode == HttpStatusCode.OK;
            //            // ReSharper disable once EmptyStatement
            //        } catch { ;}

            //        if (pageExists) {
            //            await Redis.HSetAsync<string>("yo:twitter", request.Name, "@");
            //            checkedName = "<a href=\"https://twitter.com/" + request.Name + "\">@" + request.Name + "</a>";

            //        } else {
            //            await Redis.HSetAsync<string>("yo:twitter", request.Name, "");
            //            checkedName = "" + request.Name;
            //        }
            //    } else { checkedName = checkedName + request.Name; }
            //}
            //request.Name = checkedName;

            //var messageLine = counter.TotalCounter + ". "
            //                  + checkedName + ": " + request.Message;

            //// save
            //if (!string.IsNullOrEmpty(request.Message)) {
            //    await Redis.RPushAsync<string>("yo:messages", messageLine);
            //}
            //if (!string.IsNullOrEmpty(request.Message))
            //    yoHub.Clients.All.AddMessage(messageLine);
            //yoHub.Clients.All.SetAllYos(counter.TotalCounter);
            //yoHub.Clients.User(HubUserId).SetMyYos(counter.UserCounter);

            //var resp = new YoResponse {
            //    AllYos = counter.TotalCounter,
            //    MyYos = counter.UserCounter
            //};

            //if (request.WithHistory) {
            //    var hist = await Redis.LRangeAsync<string>("yo:messages", 0, 100);
            //    resp.History = hist;
            //}

            //return resp;
            var resp =  new YoResponse {AllYos = 42, MyYos = 42, History = new[] {"Dummy history"}};
            return resp; //await Task.FromResult(resp);
        }


        // Using Ractor is optional 
        //[Authorize]
        [Route("counter")]
        public async Task<YoCounterResponse> GetCounter() {
            var countActor = new YoCounter();
            var result = await countActor
                .ParallelWith(countActor)
                .PostAndGetResultAsync(Tuple.Create<string, string>(null, User.Identity.Name));
            return new YoCounterResponse { TotalCounter = result.Item1, 
                UserCounter = result.Item2 };
        }
    }
}