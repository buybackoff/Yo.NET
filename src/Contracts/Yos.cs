namespace Yo.Contracts {

    public class YoRequest {
        public string Message { get; set; }
    }

    public class YoResponse {
        public long AllYos { get; set; }
        public long UserYos { get; set; }
    }

    public class YoHistoryResponse {
        public string[] History { get; set; }
    }

}
