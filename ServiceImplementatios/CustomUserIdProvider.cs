using System;
using Microsoft.AspNet.SignalR;

namespace ServiceImplementations {
    /// <summary>
    /// SignalR user id is ServiceStack's session id
    /// </summary>
    public class CustomUserIdProvider : IUserIdProvider {
        // TODO should use normal UserId (email, nickname) and fall back to sessionId
        // 1. userId 1-Many sessionId/connectionId (e.g. two devices)
        // 2. sessionId 1-Many connectionId (e.g. two open browser tabs)
        // if we do not have user name we at least cover the case 2.

        public string GetUserId(IRequest request) {
            var session = request.GetUserSession();
            if (session != null && !String.IsNullOrEmpty(session.UserAuthName)) {
                return session.UserAuthName;
            }
            return request.GetSessionId();
        }
    }
}