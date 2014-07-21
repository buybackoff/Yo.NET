using System.Net;
using System.Threading.Tasks;
using Contracts.ServiceModels;

namespace Services {

    public class YoService : HttpServiceBase {
        public async Task<YoResponse> Any(Yo request) {

            // call service from another service
            var counter = await ResolveService<CounterService>().Any(null);

            // get signalR HubContext
            var echoHub = GetHubContext<YoHub>();

            string checkedName;
            if (string.IsNullOrEmpty(request.Name)) {
                checkedName = "Guest";
            } else {
                checkedName = await Redis.HGetAsync<string>("yo:twitter", request.Name);
                if (checkedName == null) {
                    var httpRequest = (HttpWebRequest)WebRequest.Create("https://twitter.com/" + request.Name);
                    httpRequest.Method = WebRequestMethods.Http.Head;
                    var pageExists = false;
                    try {
                        var response = (HttpWebResponse)httpRequest.GetResponse();
                        pageExists = response.StatusCode == HttpStatusCode.OK;
                        // ReSharper disable once EmptyStatement
                    } catch { ;}

                    if (pageExists) {
                        await Redis.HSetAsync("yo:twitter", request.Name, "@");
                        checkedName = "<a href=\"https://twitter.com/" + request.Name + "\">@" + request.Name + "</a>";

                    } else {
                        await Redis.HSetAsync("yo:twitter", request.Name, "");
                        checkedName = "" + request.Name;
                    }
                } else { checkedName = checkedName + request.Name; }
            }
            request.Name = checkedName;

            var messageLine = counter.TotalCounter + ". "
                              + checkedName + ": " + request.Message;

            // save
            if (!string.IsNullOrEmpty(request.Message)) {
                await Redis.RPushAsync<string>("yo:messages", messageLine);
            }
            if (!string.IsNullOrEmpty(request.Message))
                echoHub.Clients.All.AddMessage(messageLine);
            echoHub.Clients.All.SetAllYos(counter.TotalCounter);
            echoHub.Clients.User(HubUserId).SetMyYos(counter.UserCounter);

            var resp = new YoResponse {
                AllYos = counter.TotalCounter,
                MyYos = counter.UserCounter
            };

            if (request.WithHistory) {
                var hist = await Redis.LRangeAsync<string>("yo:messages", 0, 100);
                resp.History = hist;
            }

            return resp;
        }
    }

}