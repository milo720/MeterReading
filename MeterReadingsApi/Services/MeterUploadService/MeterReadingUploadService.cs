
using MeterReadingsApi.Models.Response;
using MeterReadingsApi.Services.MeterUploadService.CsvReading;
using MeterReadingsApi.Services.MeterUploadService.DataValidator;
using MeterReadingsDatabase.Repository;
using Microsoft.SqlServer.Server;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace MeterReadingsApi.Services
{
    public class MeterReadingUploadService : IMeterReadingUploadService
    {
        public MeterReadingUploadService(IMeterReadingCsvDataValidator meterReadingValidator, IMeterReadingCsvReader meterReadingCsvReader, IMeterReadingRepositiory meterReadingRepositiory)
        {
            this.meterReadingValidator = meterReadingValidator;
            this.meterReadingCsvReader = meterReadingCsvReader;
            this.meterReadingRepositiory = meterReadingRepositiory;
        }

        private IMeterReadingCsvReader meterReadingCsvReader;
        private IMeterReadingRepositiory meterReadingRepositiory;
        private IMeterReadingCsvDataValidator meterReadingValidator;

        public MeterReadingUploadResponse ProcessMeterReadingCsv(IFormFile file)
        {
            var (parsedRecords, parseErrors, csvLines) = meterReadingCsvReader.ReadCsv(file);
            var (validatedRecords, dataError) = meterReadingValidator.ValidateCsvData(parsedRecords);
            var (existingDataErrors, addedRecords) = meterReadingRepositiory.ValidateAgainstExitingDataAndStoreMeterReading(validatedRecords);

            return new MeterReadingUploadResponse()
            {
                Errors = parseErrors.Concat(dataError).Concat(existingDataErrors),
                SuccessfullCount = addedRecords,
                UnccessfullCount = csvLines - addedRecords
            };
        }


    }
}
