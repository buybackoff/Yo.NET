using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using ServiceStack;
using ServiceStack.Caching;

namespace Services {
    public static class SignalrToServiceStackExtensions {

        public static string GetCookieValueOrDefault(this IRequest request, string key) {
            return request.Cookies.ContainsKey(key) ? request.Cookies[key].Value : null;
        }

        /// <summary>
        /// Get ServiceStack session id from cookies
        /// </summary>
        public static string GetSessionId(this IRequest request) {
            // this is exactly what SessionFeature does but using SignalR's IRequest
            var sessionOptionsString = GetCookieValueOrDefault(request, SessionFeature.SessionOptionsKey);
            var sessionOptions = sessionOptionsString.IsNullOrEmpty()
                ? new HashSet<string>()
                : sessionOptionsString.Split(',').ToHashSet();
            return sessionOptions.Contains(SessionOptions.Permanent)
                ? GetCookieValueOrDefault(request, SessionFeature.PermanentSessionId)
                : GetCookieValueOrDefault(request, SessionFeature.SessionId);
        }

        public static string GetSessionKey(this IRequest request) {
            var sessionId = request.GetSessionId();
            return sessionId == null ? null : SessionFeature.GetSessionKey(sessionId);
        }


        public static UserSession GetUserSession(this IRequest request) {
            var cache = HostContext.TryResolve<ICacheClient>();
            var sessionKey = request.GetSessionKey();
            return cache != null && sessionKey != null
                ? cache.Get<UserSession>(sessionKey)
                : (UserSession)typeof(UserSession).CreateInstance(); // TODO null?
        }
    }
}