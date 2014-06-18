using ServiceStack;

namespace ReactTS.BackEnd {
    public class CustomUserSession : AuthUserSession {
        public string CustomProperty { get; set; }
    }
}