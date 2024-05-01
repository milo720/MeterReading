using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;

namespace MeterReadingsApi.Services.MeterUploadService.CsvReading
{
    public interface IMeterReadingCsvReader
    {
        public (IEnumerable<MeterReadingCsvDataLine> csvData, IEnumerable<Error> errors, int csvLines) ReadCsv(IFormFile formFile);
    }
}