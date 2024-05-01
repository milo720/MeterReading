namespace MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels
{
    public class MeterReadingCsvDataLine
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
