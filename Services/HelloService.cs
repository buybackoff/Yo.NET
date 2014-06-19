using Contracts.ServiceModels;
using ServiceStack;

namespace ServiceImplementations {
    public class HelloService : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }
}