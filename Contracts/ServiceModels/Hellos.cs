using ServiceStack;

namespace Contracts.ServiceModels
{
    // TODO routes Auto Route Generation Strategies Routes.AddFromAssembly() 

    //Request DTO
    public class Hello
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    //Response DTO
    public class HelloResponse
    {
        public string Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }

    //Can be called via any endpoint or format, see: http://mono.servicestack.net/ServiceStack.Hello/
}

