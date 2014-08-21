using System.Runtime.Caching;
using System.Web;
using Microsoft.AspNet.SignalR;
using Ractor;

namespace Yo.Services {

    // Some dependency resolutions here
    [Authorize]
    public abstract class HubBase<T> : Hub<T> where T : class {
        public string UserName {
            get { return Context.User == null ? null : Context.User.Identity.Name; }
        }

        public MemoryCache Cache { get { return Redis.Cache; } }
        public Redis Redis { get { return Connections.GetRedis(); } }
        public Redis GetRedis(string id) { return Connections.GetRedis(id); }
        public IPocoPersistor DB { get { return Connections.GetDB(); } }
        public IPocoPersistor GetDB(string id) { return Connections.GetDB(id); }
        public IBlobPersistor BlobStorage { get { return Connections.GetBlobStorage(); } }
        public IBlobPersistor GetBlobStorage(string id) { return Connections.GetBlobStorage(id); }

    }

    public static class SystemWebExtensions {
        public static HttpContextBase GetHttpContext(this Microsoft.AspNet.SignalR.IRequest request) {
            object value;
            if (request.Environment.TryGetValue(typeof(HttpContextBase).FullName, out value)) {
                return (HttpContextBase)value;
            }
            return null;
        }
    }
}