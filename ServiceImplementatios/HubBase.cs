using System;
using System.Web;
using Fredis;
using Microsoft.AspNet.SignalR;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Web;

namespace ServiceImplementations {

    // Some dependency resolutions here
    public abstract class HubBase<T> : Hub<T> where T : class {
        private ICacheClient _cache;
        public ICacheClient Cache {
            get {
                return _cache ?? (_cache =
                    HostContext.TryResolve<ICacheClient>());
            }
        }

        private Redis _redis;
        public Redis Redis {
            get {
                return _redis ?? (_redis =
                    HostContext.TryResolve<Redis>());
            }
        }

        public UserSession UserSession {
            get {
                var sessionKey = Context.Request.GetSessionKey();
                if (sessionKey != null && Cache != null) {
                    // session could change in the cache, do not store it in hub in a private field
                    return Cache.Get<UserSession>(sessionKey);
                }
                return null;
            }
        }

        public HttpContextBase HttpContextBase {
            get { return Context.Request.GetHttpContext(); }
        }


        protected TService ResolveService<TService>() where TService : class, IService {
            var service = HostContext.TryResolve<TService>();
            if (service == null) return null;
            var requiresContext = service as IRequiresRequest;
            if (requiresContext == null) return service;

            var ctx = Context.Request.GetHttpContext();
            var httpReq = ctx == null ? null : ctx.ToRequest();
            if (httpReq == null) throw new ApplicationException("Cannot provide request to a service that requires request.");
            requiresContext.Request = httpReq;
            return service;
        }

    }

}