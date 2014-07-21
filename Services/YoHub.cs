using System;
using System.Threading.Tasks;
using Contracts.ServiceModels;

namespace Services {

    public interface IEchoHubClient {
        void SetAllYos(long totalCounter);
        void SetMyYos(long userCounter);
        void AddMessage(string messageLine);
    }


    public class YoHub : HubBase<IEchoHubClient> {
        public async Task<string> Yo(string name, string message) {
            
            try {
                var srv = ResolveService<YoService>();
                await srv.Any(new Yo { Name = name, Message = message });
                return "Yo";
            } catch (Exception e) {
                return "not Yo! " + e.Message;
            }
            
        }

    }
}