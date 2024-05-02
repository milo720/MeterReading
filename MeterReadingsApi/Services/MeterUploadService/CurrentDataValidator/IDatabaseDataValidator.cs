using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;

namespace MeterReadingsApi.Services.MeterUploadService.CurrentDataValidator
{
    public interface IDatabaseDataValidator
    {
        (IEnumerable<MeterReadingCsvDataLine> csvData, IEnumerable<Error> errors) ValidateAgianstExitingData(IEnumerable<MeterReadingCsvDataLine> csvData);
    }
}