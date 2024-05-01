using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingsDatabase.Models
{
    public class Account
    {
        [Key]
        public int AccountId
        {
            get;
            set;
        }
        public string EmployeeName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }

        DbSet<MeterReading> MeterReadings { get; set; }
    }
}
