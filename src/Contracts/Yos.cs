namespace Yo.Contracts {

    public class YoRequest {
        public string Name { get; set; }
        public string Message { get; set; }
        public bool WithHistory { get; set; }
    }

    public class YoResponse {
        public long AllYos { get; set; }
        public long MyYos { get; set; }
        public string[] History { get; set; }
    }

    public class YoCounterRequest { }
    public class YoCounterResponse {
        public long TotalCounter { get; set; }
        public long UserCounter { get; set; }
    }

}
