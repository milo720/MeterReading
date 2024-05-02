using MeterReadingsDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingsDatabase
{
    public class MeterReadingDbContext : DbContext
    {


        public MeterReadingDbContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Account> Accounts
        {
            get;
            set;
        }

        public virtual DbSet<MeterReading> MeterReadings
        {
            get;
            set;
        }
    }
}
