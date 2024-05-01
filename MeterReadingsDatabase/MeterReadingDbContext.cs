using MeterReadingsDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingsDatabase
{
    public class MeterReadingDbContext : DbContext
    {
        public MeterReadingDbContext(DbContextOptions options) : base(options) { }
        DbSet<Account> Accounts
        {
            get;
            set;
        }

        DbSet<MeterReading> MeterReadings
        {
            get;
            set;
        }
    }
}
