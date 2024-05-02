using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;
using MeterReadingsDatabase;
using MeterReadingsDatabase.Models;
using MeterReadingsDatabase.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var groupedReadings = meterReadings.GroupBy(c => c.AccountId);
            var duplicationErrors = groupedReadings.Where(c => c.Count() > 1).Select(c => c.OrderByDescending(s => s.MeterReadingDateTime).Skip(1)).SelectMany(c => c).Select(c => new Error()
            {
                Message = "There is more than one entry for this account in this import, as such the newest has been used",
                Data = c.AccountId,
                Source = "AccountId"
            }).ToList();
            var deduplicatedMeterReadings = groupedReadings.Select(c => c.MaxBy(c => c.MeterReadingDateTime));
            var accountIds = deduplicatedMeterReadings.Select(c => c.AccountId);
            int addedRecords = 0;
            var errors = new List<Error>(duplicationErrors);
            var currentAccounts = readingDbContext.Accounts.Include(u => u.MeterReadings).Where(c => accountIds.Contains(c.AccountId)).ToDictionary(c =>c.AccountId);
            using var transaction = readingDbContext.Database.BeginTransaction();
            foreach (var reading in deduplicatedMeterReadings)
            {
                if(!currentAccounts.ContainsKey(reading.AccountId))
                {
                    errors.Add(new Error()
                    {
                        Message = "There is no account with this ID",
                        Source = "AccountId",
                        Data = reading.AccountId
                    });
                }
                else if(currentAccounts[reading.AccountId].MeterReadings.IsNullOrEmpty())
                {
                    currentAccounts[reading.AccountId].MeterReadings.Add(new MeterReading()
                    {
                        MeterReadingDateTime = reading.MeterReadingDateTime,
                        MeterReadValue = int.Parse(reading.MeterReadValue)
                    });
                    addedRecords += 1;
                    readingDbContext.SaveChanges();
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
                    currentAccounts[reading.AccountId].MeterReadings.Add(new MeterReading()
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
