using FluentValidation;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;


namespace MeterReadingsApi.Services.MeterUploadService.DataValidator
{
    public class MeterReadingCsvContentValidator : IMeterReadingCsvDataValidator
    {
        public MeterReadingCsvContentValidator(AbstractValidator<MeterReadingCsvDataLine> meterReadingValidator)
        {
            MeterReadingValidator = meterReadingValidator;
        }

        public AbstractValidator<MeterReadingCsvDataLine> MeterReadingValidator { get; }

        public (IEnumerable<MeterReadingCsvDataLine> csvData, IEnumerable<Error> errors) ValidateCsvData(IEnumerable<MeterReadingCsvDataLine> records)
        {
            var errors = new List<Error>();
            var validRecords = new List<MeterReadingCsvDataLine>();
            foreach (var record in records)
            {
                var validation = MeterReadingValidator.Validate(record);
                if (validation.IsValid)
                {
                    validRecords.Add(record);
                }
                else
                {
                    foreach (var error in validation.Errors)
                    {
                        errors.Add(new Error()
                        {
                            Message = error.ErrorMessage,
                            Data = error.AttemptedValue,
                            Source = error.PropertyName
                        });
                    }
                }
            }

            var groupedReadings = validRecords.GroupBy(c => c.AccountId);
            var duplicationErrors = groupedReadings.Where(c => c.Count() > 1).Select(c => c.OrderByDescending(s => s.MeterReadingDateTime).Skip(1)).SelectMany(c => c).Select(c => new Error()
            {
                Message = "There is more than one entry for this account in this import, as such the newest has been used",
                Data = c,
                Source = "AccountId"
            }).ToList();
            var deduplicatedMeterReadings = groupedReadings.Select(c => c.MaxBy(c => c.MeterReadingDateTime));


            return (deduplicatedMeterReadings, errors.Concat(duplicationErrors));
        }
    }
}
