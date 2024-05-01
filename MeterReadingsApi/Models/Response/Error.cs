namespace MeterReadingsApi.Models.Response
{
    public class Error
    {
        public string Source { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
