using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Services {

    // TODO test (rewrite auth logic to use SS with SR's Request)
    // In SR's source code the only attributes used are the ones that implement the 
    // interfaces, so instead of inheriting from SR's authorize attribute we could write our 
    // own and use SS's auth.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute, IAuthorizeHubConnection, IAuthorizeHubMethodInvocation {
        private string _roles;
        private string[] _rolesSplit = new string[0];
        private string _users;
        private string[] _usersSplit = new string[0];

        protected bool? Outgoing;

        /// <summary>
        /// Set to false to apply authorization only to the invocations of any of the Hub's server-side methods.
        /// This property only affects attributes applied to the Hub class.
        /// This property cannot be read.
        /// </summary>
        public bool RequireOutgoing {
            // It is impossible to tell here whether the attribute is being applied to a method or class. This makes
            // it impossible to determine whether the value should be true or false when _requireOutgoing is null.
            // It is also impossible to have a Nullable attribute parameter type.
            get { throw new NotImplementedException(""); }
            set { Outgoing = value; }
        }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        public string Roles {
            get { return _roles ?? String.Empty; }
            set {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }

        /// <summary>
        /// Gets or sets the authorized users.
        /// </summary>
        public string Users {
            get { return _users ?? String.Empty; }
            set {
                _users = value;
                _usersSplit = SplitString(value);
            }
        }

        /// <summary>
        /// Determines whether client is authorized to connect to <see cref="IHub"/>.
        /// </summary>
        /// <param name="hubDescriptor">Description of the hub client is attempting to connect to.</param>
        /// <param name="request">The (re)connect request from the client.</param>
        /// <returns>true if the caller is authorized to connect to the hub; otherwise, false.</returns>
        public virtual bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            // If RequireOutgoing is explicitly set to false, authorize all connections.
            if (Outgoing.HasValue && !Outgoing.Value) {
                return true;
            }

            return IsUserAuthorized(request);
        }

        /// <summary>
        /// Determines whether client is authorized to invoke the <see cref="IHub"/> method.
        /// </summary>
        /// <param name="hubIncomingInvokerContext">An <see cref="IHubIncomingInvokerContext"/> providing details regarding the <see cref="IHub"/> method invocation.</param>
        /// <param name="appliesToMethod">Indicates whether the interface instance is an attribute applied directly to a method.</param>
        /// <returns>true if the caller is authorized to invoke the <see cref="IHub"/> method; otherwise, false.</returns>
        public virtual bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod) {
            if (hubIncomingInvokerContext == null) {
                throw new ArgumentNullException("hubIncomingInvokerContext");
            }

            // It is impossible to require outgoing auth at the method level with SignalR's current design.
            // Even though this isn't the stage at which outgoing auth would be applied, we want to throw a runtime error
            // to indicate when the attribute is being used with obviously incorrect expectations.

            // We must explicitly check if _requireOutgoing is true since it is a Nullable type.
            if (appliesToMethod && (Outgoing == true)) {
                throw new ArgumentException("Resources.Error_MethodLevelOutgoingAuthorization");
            }

            return IsUserAuthorized(hubIncomingInvokerContext.Hub.Context.Request);
        }

        /// <summary>
        /// Checks is the user making request is authorized
        /// </summary>
        protected virtual bool IsUserAuthorized(IRequest request) {
            var session = request.GetUserSession();
            if (session == null) {
                return false;
            }
            if (!session.IsAuthenticated) {
                return false;
            }

            if (_usersSplit.Length > 0 && !_usersSplit.Contains(session.UserAuthName, StringComparer.OrdinalIgnoreCase)) {
                return false;
            }

            if (_rolesSplit.Length > 0 && !_rolesSplit.Any(session.HasRole)) {
                return false;
            }

            return true;
        }

        private static string[] SplitString(string original) {
            if (String.IsNullOrEmpty(original)) {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}
