using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsDatabase.Models;


namespace MeterReadingsDatabase.Repository
{
    public interface IMeterReadingRepositiory
    {
        public void UploadRedings(IEnumerable<MeterReadingCsvDataLine> meterReadings);

        public IEnumerable<Account> GetAccounts(IEnumerable<int> IDs);
    }
}
