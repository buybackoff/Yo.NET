using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ServiceStack;

namespace ServiceImplementations.Common {

    // since we base all out auth on ServiceStack,
    // we could have mapping between UserSession and SignalR's UserId

    public class CustomUserIdProvider : IUserIdProvider {
        public string GetUserId(IRequest request) {
            
            // TODO this could fail to work
            // use cookies to get SS's session key if that fails
            if (HttpContext.Current == null) throw new ApplicationException("Session key won't work");
            var id = SessionFeature.GetSessionId();
            return id ?? ""; // never null
        }
    }
}
