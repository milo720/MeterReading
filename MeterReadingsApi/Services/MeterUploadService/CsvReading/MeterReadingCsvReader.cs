using CsvHelper.Configuration;
using CsvHelper;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using System.Globalization;
using MeterReadingsApi.Models.Response;
using System.Diagnostics.CodeAnalysis;


namespace MeterReadingsApi.Services.MeterUploadService.CsvReading
{
    public class MeterReadingCsvReader : IMeterReadingCsvReader
    {
        [ExcludeFromCodeCoverage(Justification = "Testing the CSV reader is possible by fakeing I formfile and returning a test stream with test CSV Data, due to time constraints I'm not doing this.")]
        public (IEnumerable<MeterReadingCsvDataLine> csvData, IEnumerable<Error> errors, int csvLines) ReadCsv(IFormFile formFile)
        {
            var errors = new List<Error>();
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
            
            configuration.BadDataFound = context =>
            {
                errors.Add(new Error()
                {
                    Message = "Could not parse CSV Line",
                    Data = context.RawRecord,
                    Source = "Csv"
                });
            };
            configuration.ReadingExceptionOccurred = excpetion =>
            {

                errors.Add(new Error()
                {
                    Message = "Could not parse CSV Line",
                    Source = "Csv"
                });
                return false;
            };

            using (var stream = formFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap<MeterReadingCsvDataLineMap>();
                var records = csv.GetRecords<MeterReadingCsvDataLine>().ToList();
                return (records, errors, records.Count() + errors.Count());
            }
            
        }
    }
}
