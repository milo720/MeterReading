using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;
using MeterReadingsDatabase.Repository;
using Microsoft.IdentityModel.Tokens;

namespace MeterReadingsApi.Services.MeterUploadService.CurrentDataValidator
{
    public class DatabaseDataValidator : IDatabaseDataValidator
    {
        private readonly IMeterReadingRepositiory meterReadingRepositiory;

        public DatabaseDataValidator(IMeterReadingRepositiory meterReadingRepositiory)
        {
            this.meterReadingRepositiory = meterReadingRepositiory;
        }
        public (IEnumerable<MeterReadingCsvDataLine> csvData, IEnumerable<Error> errors) ValidateAgianstExitingData(IEnumerable<MeterReadingCsvDataLine> csvData)
        {
            var currentAccounts = meterReadingRepositiory.GetAccounts(csvData.Select(c => c.AccountId)).ToDictionary(c => c.AccountId, c => c);
            var errors = new List<Error>();
            var csvDataValidAgiantDb = new List<MeterReadingCsvDataLine>();
            foreach (var reading in csvData)
            {
                if (!currentAccounts.ContainsKey(reading.AccountId))
                {
                    errors.Add(new Error()
                    {
                        Message = "There is no account with this ID",
                        Source = "AccountId",
                        Data = reading.AccountId
                    });
                }
                else if (currentAccounts[reading.AccountId].MeterReadings.IsNullOrEmpty())
                {
                    csvDataValidAgiantDb.Add(reading);
                }
                else if (currentAccounts[reading.AccountId]?.MeterReadings.Max(c => c.MeterReadingDateTime) == reading.MeterReadingDateTime)
                {
                    errors.Add(new Error()
                    {
                        Message = "A reading already exists for this user at the same time",
                        Source = reading.AccountId.ToString(),
                        Data = reading.MeterReadingDateTime
                    });
                }
                else if (currentAccounts[reading.AccountId]?.MeterReadings.Max(c => c.MeterReadingDateTime) > reading.MeterReadingDateTime)
                {
                    errors.Add(new Error()
                    {
                        Message = "A reading already exists after this reading for the user",
                        Source = reading.AccountId.ToString(),
                        Data = reading.MeterReadingDateTime
                    });
                }
                else
                {
                    csvDataValidAgiantDb.Add(reading);
                }
            }
            return (csvDataValidAgiantDb,errors);


        }
    }
    
}
