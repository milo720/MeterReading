using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReadingsDatabase.Models
{
    public class MeterReading
    {
        [Key]
        public int MeterReadingId { get; set; }
        public DateTime MeterReadingDateTime
        {
            get;
            set;
        }
        public int MeterReadValue
        {
            get;
            set;
        }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}