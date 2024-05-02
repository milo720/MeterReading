using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;

namespace MeterReadingsApi.Services.MeterUploadService.DataValidator
{
    public interface IMeterReadingCsvDataValidator
    {
        public (IEnumerable<MeterReadingCsvDataLine> csvData, IEnumerable<Error> errors) ValidateCsvData(IEnumerable<MeterReadingCsvDataLine> records);
    }
}
