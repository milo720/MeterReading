
using MeterReadingsApi.Models.Response;
using MeterReadingsApi.Services.MeterUploadService.CsvReading;
using MeterReadingsApi.Services.MeterUploadService.CurrentDataValidator;
using MeterReadingsApi.Services.MeterUploadService.DataValidator;
using MeterReadingsDatabase.Repository;

namespace MeterReadingsApi.Services
{
    public class MeterReadingUploadService : IMeterReadingUploadService
    {
        public MeterReadingUploadService(IMeterReadingCsvDataValidator meterReadingValidator, IMeterReadingCsvReader meterReadingCsvReader, IDatabaseDataValidator databaseDataValidator, IMeterReadingRepositiory meterReadingRepositiory)
        {
            this.meterReadingValidator = meterReadingValidator;
            this.meterReadingCsvReader = meterReadingCsvReader;
            this.databaseDataValidator = databaseDataValidator;
            this.meterReadingRepositiory = meterReadingRepositiory;
        }

        private IMeterReadingCsvReader meterReadingCsvReader;
        private readonly IDatabaseDataValidator databaseDataValidator;
        private IMeterReadingRepositiory meterReadingRepositiory;
        private IMeterReadingCsvDataValidator meterReadingValidator;

        public MeterReadingUploadResponse ProcessMeterReadingCsv(IFormFile file)
        {
            var (parsedRecords, parseErrors, csvLines) = meterReadingCsvReader.ReadCsv(file);
            var (recprdsWithValidStructure, dataError) = meterReadingValidator.ValidateCsvData(parsedRecords);
            var (validatedRecords, errorsAgainstCurrentData) = databaseDataValidator.ValidateAgianstExitingData(recprdsWithValidStructure);
            meterReadingRepositiory.UploadRedings(validatedRecords);

            return new MeterReadingUploadResponse()
            {
                Errors = parseErrors.Concat(dataError).Concat(errorsAgainstCurrentData),
                SuccessfullCount = validatedRecords.Count(),
                UnccessfullCount = csvLines - validatedRecords.Count()
            };
        }


    }
}
