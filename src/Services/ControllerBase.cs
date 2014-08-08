using System.Runtime.Caching;
using System.Web.Http;
using Ractor;

namespace Yo.Services {
    public class ControllerBase : ApiController {

        public string UserName {
            get { return User == null ? null : User.Identity.Name; }
        }

        public MemoryCache Cache { get { return Redis.Cache; } }
        public Redis Redis { get { return Connections.GetRedis(); } }
        public Redis GetRedis(string id) { return Connections.GetRedis(id); }
        public IPocoPersistor DB { get { return Connections.GetDB(); } }
        public IPocoPersistor GetDB(string id) { return Connections.GetDB(id); }
        public IBlobPersistor BlobStorage { get { return Connections.GetBlobStorage(); } }
        public IBlobPersistor GetBlobStorage(string id) { return Connections.GetBlobStorage(id); }

    }
}
