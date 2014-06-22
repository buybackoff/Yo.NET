using Fredis;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ServiceStack;

namespace ServiceImplementations.Common {
    public abstract class ServiceBase : Service {
        private Redis _redis;
        public virtual Redis Redis {
            get {
                return _redis ?? (_redis =
                    HostContext.TryResolve<Redis>());
            }
        }

        public UserSession UserSession {
            get {
                return base.SessionAs<UserSession>();
            }
        }

        public string SessionId {
            get { return SessionFeature.GetSessionId(Request); }
        }

        public string SessionKey {
            get { return SessionFeature.GetSessionKey(Request); }
        }

        public IHubContext GetHubContext<THub>()
            where THub : HubBase {
            return GlobalHost.ConnectionManager.GetHubContext<THub>();
        }
    }


}
