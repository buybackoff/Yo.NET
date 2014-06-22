using System.Threading.Tasks;
using System.Web;
using Fredis;
using Microsoft.AspNet.SignalR;
using ServiceStack;
using ServiceStack.Caching;

namespace ServiceImplementations.Common {

    // Some dependency resolutions here
    public abstract class HubBase<T> : Hub<T> where T : class {
        private ICacheClient _cache;
        public virtual ICacheClient Cache {
            get {
                return _cache ?? (_cache =
                    HostContext.TryResolve<ICacheClient>());
            }
        }

        private Redis _redis;
        public virtual Redis Redis {
            get {
                return _redis ?? (_redis =
                    HostContext.TryResolve<Redis>());
            }
        }

        private UserSession _session;
        public virtual UserSession UserSession {
            get {
                return _session ?? (_session =
                    Cache.SessionAs<UserSession>());
            }
        }

        public HttpContextBase HttpContextBase {
            get { return this.Context.Request.GetHttpContext(); }
        }

    }

    //https://github.com/SignalR/SignalR/releases
    public static class SystemWebExtensions {
        public static HttpContextBase GetHttpContext(this IRequest request) {
            object value;
            if (request.Environment.TryGetValue(typeof(HttpContextBase).FullName, out value)) {
                return (HttpContextBase)value;
            }
            return null;
        }
    }

}