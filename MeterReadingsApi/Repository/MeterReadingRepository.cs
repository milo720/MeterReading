using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;
using MeterReadingsDatabase;
using MeterReadingsDatabase.Models;
using MeterReadingsDatabase.Repository;
using System.Diagnostics.CodeAnalysis;

namespace MeterReadingsApi.Repository
{
    [ExcludeFromCodeCoverage(Justification ="While possible I am going to avoid unit testing in the coding test. The funtionality will e covered by the Intergration tests.")]
    public class MeterReadingRepository: IMeterReadingRepositiory
    {
        private readonly MeterReadingDbContext readingDbContext;

        public MeterReadingRepository(MeterReadingDbContext readingDbContext) {
            this.readingDbContext = readingDbContext;
        }


        public (IEnumerable<Error> errors, int addedRecords) ValidateAgainstExitingDataAndStoreMeterReading(IEnumerable<MeterReadingCsvDataLine> meterReadings)
        {
            var accountIds = meterReadings.Select(c => c.AccountId);
            int addedRecords = 0;
            var errors = new List<Error>();
            var currentAccounts = readingDbContext.MeterReadings.Where(c => accountIds.Contains(c.Account.AccountId)).GroupBy(c => c.MeterReadingId, (key, values) => values.MaxBy(c => c.MeterReadingDateTime)).ToDictionary(c => c.Account.AccountId, c => c );
            using var transaction = readingDbContext.Database.BeginTransaction();
            foreach (var reading in  meterReadings)
            {
                if(!currentAccounts.ContainsKey(reading.AccountId))
                {

                }
                else if (currentAccounts[reading.AccountId].MeterReadingDateTime == reading.MeterReadingDateTime)
                {
                    errors.Add(new Error()
                    {
                        Message = "A reading already exists for this user at the same time",
                        Source = reading.AccountId.ToString(),
                        Data = reading.MeterReadingDateTime
                    });
                }
                else if (currentAccounts[reading.AccountId].MeterReadingDateTime > reading.MeterReadingDateTime)
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
                    currentAccounts[reading.AccountId].Account.MeterReadings.Add(new MeterReading()
                    {
                        MeterReadingDateTime = reading.MeterReadingDateTime,
                        MeterReadValue = int.Parse(reading.MeterReadValue)
                    });
                    addedRecords += 1;
                    readingDbContext.SaveChanges();
                }
            }
            transaction.Commit();
            return (errors, addedRecords);


        }
    }
}
