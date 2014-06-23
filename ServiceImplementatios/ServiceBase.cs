using System;
using System.Data;
using Fredis;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.Redis;
using ServiceStack.Web;

namespace ServiceImplementations {

    // TODO All stuff from AppStart as a property, e.g. PocoPersistor

    public abstract class ServiceBase : IService {
        private Redis _redis;
        public virtual Redis Redis {
            get {
                return _redis ?? (_redis =
                    HostContext.TryResolve<Redis>());
            }
        }


        public IHubContext GetHubContext<THub>()
            where THub : HubBase {
            return GlobalHost.ConnectionManager.GetHubContext<THub>();
        }


        public virtual T TryResolve<T>() {
            return HostContext.TryResolve<T>();
        }

        public virtual T ResolveService<T>() {
            var service = TryResolve<T>();
            var requiresContext = service as IRequiresRequest;
            if (requiresContext == null) return service;
            throw new ApplicationException("Cannot provide request to a service that requires request.");
        }


        private ICacheClient _cache;
        public virtual ICacheClient Cache {
            get {
                return _cache ??
                    (_cache = TryResolve<ICacheClient>()) ??
                    (_cache = (TryResolve<IRedisClientsManager>() != null ? TryResolve<IRedisClientsManager>().GetCacheClient() : null));
            }
        }

        private IDbConnection _db;
        public virtual IDbConnection Db {
            get { return _db ?? (_db = TryResolve<IDbConnectionFactory>().OpenDbConnection()); }
        }
        
        private ISessionFactory _sessionFactory;
        public virtual ISessionFactory SessionFactory {
            get { return _sessionFactory ?? (_sessionFactory = TryResolve<ISessionFactory>()) ?? new SessionFactory(Cache); }
        }

        public virtual void Dispose() {
            if (_db != null)
                _db.Dispose();
        }
    }


    public abstract class HttpServiceBase : ServiceBase, IRequiresRequest {
        public ServiceStack.Web.IRequest Request { get; set; }

        protected virtual IResponse Response {
            get { return Request != null ? Request.Response : null; }
        }

        public UserSession UserSession {
            get {
                return SessionAs<UserSession>();
            }
        }

        /// <summary>
        /// SignalR user id
        /// </summary>
        public string HubUserId {
            get {
                var session = UserSession;
                if (session != null && !String.IsNullOrEmpty(session.UserAuthName)) { return session.UserAuthName; }
                return SessionId;
            }
        }

        public string SessionId {
            get { return SessionFeature.GetSessionId(Request); }
        }

        public string SessionKey {
            get { return SessionFeature.GetSessionKey(Request); }
        }

        public virtual bool IsAuthenticated {
            get { return UserSession.IsAuthenticated; }
        }

        /// <summary>
        /// Typed UserSession
        /// </summary>
        protected virtual TUserSession SessionAs<TUserSession>() {
            var ret = TryResolve<TUserSession>();
            return !Equals(ret, default(TUserSession))
                ? ret
                : Cache.SessionAs<TUserSession>(Request, Response);
        }

        public override T ResolveService<T>() {
            var service = TryResolve<T>();
            var requiresContext = service as IRequiresRequest;
            if (requiresContext != null) {
                requiresContext.Request = Request;
            }
            return service;
        }
    }
}
