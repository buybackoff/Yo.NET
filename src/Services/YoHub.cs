using System;
using System.Threading.Tasks;

namespace Yo.Services {

    public interface IYoHubClient {
        void SetAllYos(long totalCounter);
        void SetMyYos(long userCounter);
        void AddMessage(string messageLine);
    }

    public class YoHub : HubBase<IYoHubClient> {
        public async Task<string> Yo(string name, string message) {
            try {
                //await YoController. srv.Any(new YoRequest { Name = name, Message = message });
                return "Yo";
            } catch (Exception e) {
                return "not Yo! " + e.Message;
            }
        }
    }

}