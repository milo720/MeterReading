using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsDatabase;
using MeterReadingsDatabase.Models;
using MeterReadingsDatabase.Repository;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Account> GetAccounts(IEnumerable<int> IDs)
        {
            return readingDbContext.Accounts.Include(u => u.MeterReadings).Where(c => IDs.Contains(c.AccountId)).ToList();
        }

        public void UploadRedings(IEnumerable<MeterReadingCsvDataLine> meterReadings) { 

            using var transaction = readingDbContext.Database.BeginTransaction();
            foreach (var reading in meterReadings)
            {

                readingDbContext.MeterReadings.Add(new MeterReading()
                {
                    MeterReadingDateTime = reading.MeterReadingDateTime,
                    MeterReadValue = int.Parse(reading.MeterReadValue),
                    AccountId = reading.AccountId,
                });
                readingDbContext.SaveChanges();

            }

        }
    }
}
