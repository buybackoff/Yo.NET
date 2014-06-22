using ServiceStack;

namespace Contracts.ServiceModels {
    //Request DTO
    [Route("/yo/")]
    [Route("/yo/{Message}")]
    public class Yo {
        public string Name { get; set; }
        public string Message { get; set; }
        public bool WithHistory { get; set; }
    }


    // TODO service to get all messages, paginated, use infinite scroll on teh client side

    //Response DTO
    public class YoResponse {
        public long AllYos { get; set; }
        public long MyYos { get; set; }
        public string[] History { get; set; }
        public ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }
}
