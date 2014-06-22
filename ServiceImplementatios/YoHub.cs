using System.Threading.Tasks;
using Contracts.ServiceModels;
using ServiceImplementations.Common;
using ServiceStack;

namespace ServiceImplementations {

    public interface IEchoHubClient {
        void SetAllYos(long totalCounter);
        void SetMyYos(long userCounter);
        void AddMessage(string messageLine);
    }


    public class YoHub : HubBase<IEchoHubClient> {
        public async Task<string> Yo(string name, string message) {
            var srv = HostContext.ResolveService<YoService>(HttpContextBase);
            await srv.Any(new Yo {Name = name, Message = message});
            return "Yo";
        }

    }
}