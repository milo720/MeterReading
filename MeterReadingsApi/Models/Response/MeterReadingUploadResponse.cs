namespace MeterReadingsApi.Models.Response
{
    public class MeterReadingUploadResponse
    {
        public int SuccessfullCount { get; set; }
        public int UnccessfullCount { get; set; }

        public IEnumerable<Error> Errors {get;set;} = new List<Error>();
    }
}